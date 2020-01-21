namespace Newsweek.Worker.Core.Tasks
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
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Handlers.Queries.News;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly IEnumerable<INewsProvider> newsProviders;
        private readonly IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<News, int>, Task<IEnumerable<News>>> newsCreateHandler;
        private readonly ICommandHandler<CreateSubcategoriesCommand, Task<IEnumerable<Subcategory>>> subcategoriesCreateHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<NewsSubcategory, int>, Task<IEnumerable<NewsSubcategory>>> newsSubcategoriesCreateHandler;

        public NewsTask(
            IEnumerable<INewsProvider> newsProviders,
            IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler,
            ICommandHandler<CreateEntitiesCommand<News, int>, Task<IEnumerable<News>>> newsCreateHandler,
            ICommandHandler<CreateSubcategoriesCommand, Task<IEnumerable<Subcategory>>> subcategoriesCreateHandler,
            ICommandHandler<CreateEntitiesCommand<NewsSubcategory, int>, Task<IEnumerable<NewsSubcategory>>> newsSubcategoriesCreateHandler)
        {
            this.newsProviders = newsProviders;
            this.newsGetHandler = newsGetHandler;
            this.newsCreateHandler = newsCreateHandler;
            this.subcategoriesCreateHandler = subcategoriesCreateHandler;
            this.newsSubcategoriesCreateHandler = newsSubcategoriesCreateHandler;
        }

        public async Task DoWork()
        {
            List<CreateNewsCommand> newsCommands = new List<CreateNewsCommand>();

            foreach (var newsProvider in newsProviders)
            {
                IEnumerable<CreateNewsCommand> commands = await newsProvider.Get();
                newsCommands.AddRange(commands);
            }

            var commandSubcategories = newsCommands.Select(x => x.Subcategory)
                .GroupBy(x => x.Name)
                .Select(x => x.First());

            IEnumerable<Subcategory> subcategories = await subcategoriesCreateHandler.Handle(new CreateSubcategoriesCommand(commandSubcategories));
            IEnumerable<News> news = await CreateNews(newsCommands);

            await CreateNewsSubcategories(newsCommands, news, subcategories);
        }

        private async Task<IEnumerable<News>> CreateNews(IEnumerable<CreateNewsCommand> newsCommands)
        {
            ICollection<CreateNewsCommand> newsCommandsToCreate = new List<CreateNewsCommand>();

            IEnumerable<string> urls = newsCommands.Select(x => x.RemoteUrl);
            IEnumerable<News> news = await newsGetHandler.Handle(new NewsByRemoteUrlQuery(urls));

            foreach (var command in newsCommands)
            {
                if (news == null || !news.Any(x => x.RemoteUrl == command.RemoteUrl))
                {
                    newsCommandsToCreate.Add(command);
                }
            }

            return await newsCreateHandler.Handle(new CreateEntitiesCommand<News, int>(newsCommandsToCreate));
        }

        private async Task CreateNewsSubcategories(IEnumerable<CreateNewsCommand> newsCommands, IEnumerable<News> news, IEnumerable<Subcategory> subcategories)
        {
            var newsSubcategoriesCommands = new List<CreateNewsSubcategoryCommand>();

            foreach (var article in news)
            {
                CreateNewsCommand newsCommand = newsCommands.FirstOrDefault(x => x.RemoteUrl == article.RemoteUrl);

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