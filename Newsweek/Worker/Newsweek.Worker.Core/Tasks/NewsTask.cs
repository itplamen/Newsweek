﻿namespace Newsweek.Worker.Core.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.NewsSubcategories;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly IEnumerable<INewsProvider> newsProviders;
        private readonly ICommandHandler<CreateNewsCommand, Task<IEnumerable<News>>> newsCreateHandler;
        private readonly ICommandHandler<CreateSubcategoriesCommand, Task<IEnumerable<Subcategory>>> subcategoriesCreateHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<NewsSubcategory, int>, Task<IEnumerable<NewsSubcategory>>> newsSubcategoriesCreateHandler;

        public NewsTask(
            IEnumerable<INewsProvider> newsProviders,
            ICommandHandler<CreateNewsCommand, Task<IEnumerable<News>>> newsCreateHandler,
            ICommandHandler<CreateSubcategoriesCommand, Task<IEnumerable<Subcategory>>> subcategoriesCreateHandler,
            ICommandHandler<CreateEntitiesCommand<NewsSubcategory, int>, Task<IEnumerable<NewsSubcategory>>> newsSubcategoriesCreateHandler)
        {
            this.newsProviders = newsProviders;
            this.newsCreateHandler = newsCreateHandler;
            this.subcategoriesCreateHandler = subcategoriesCreateHandler;
            this.newsSubcategoriesCreateHandler = newsSubcategoriesCreateHandler;
        }

        public async Task DoWork()
        {
            List<NewsCommand> newsCommands = new List<NewsCommand>();

            foreach (var newsProvider in newsProviders)
            {
                IEnumerable<NewsCommand> commands = await newsProvider.Get();
                newsCommands.AddRange(commands);
            }

            newsCommands = newsCommands.GroupBy(x => x.RemoteUrl)
                .Where(x => x.Count() == 1)
                .SelectMany(x => x)
                .ToList();

            var commandSubcategories = newsCommands.Select(x => x.Subcategory)
                .GroupBy(x => x.Name)
                .Select(x => x.First());

            IEnumerable<Subcategory> subcategories = await subcategoriesCreateHandler.Handle(new CreateSubcategoriesCommand(commandSubcategories));
            IEnumerable<News> news = await newsCreateHandler.Handle(new CreateNewsCommand(newsCommands));

            await CreateNewsSubcategories(newsCommands, news, subcategories);
        }

        private async Task CreateNewsSubcategories(IEnumerable<NewsCommand> newsCommands, IEnumerable<News> news, IEnumerable<Subcategory> subcategories)
        {
            var newsSubcategoriesCommands = new List<CreateNewsSubcategoryCommand>();

            foreach (var article in news)
            {
                NewsCommand newsCommand = newsCommands.FirstOrDefault(x => x.RemoteUrl == article.RemoteUrl);

                if (newsCommand != null)
                {
                    Subcategory subcategory = subcategories.FirstOrDefault(x => x.Name == newsCommand.Subcategory.Name);

                    if (subcategory != null)
                    {
                        newsSubcategoriesCommands.Add(new CreateNewsSubcategoryCommand(article.Id, subcategory.Id));
                    }
                }
            }

            await newsSubcategoriesCreateHandler.Handle(new CreateEntitiesCommand<NewsSubcategory, int>(newsSubcategoriesCommands));
        }
    }
}