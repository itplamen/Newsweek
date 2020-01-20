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
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Handlers.Queries.News;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly IEnumerable<INewsProvider> newsProviders;
        private readonly IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<News, int>, Task<IEnumerable<News>>> newsCreateHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<Subcategory, int>, Task<IEnumerable<Subcategory>>> subcategoriesCreateHandler;
        private readonly IQueryHandler<EntitiesByNameQuery<Subcategory, int>, Task<IEnumerable<Subcategory>>> subcategoriesGetHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<NewsSubcategory, int>, Task<IEnumerable<NewsSubcategory>>> newsSubcategoriesCreateHandler;

        public NewsTask(
            IEnumerable<INewsProvider> newsProviders,
            IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler,
            ICommandHandler<CreateEntitiesCommand<News, int>, Task<IEnumerable<News>>> newsCreateHandler,
            ICommandHandler<CreateEntitiesCommand<Subcategory, int>, Task<IEnumerable<Subcategory>>> subcategoriesCreateHandler,
            IQueryHandler<EntitiesByNameQuery<Subcategory, int>, Task<IEnumerable<Subcategory>>> subcategoriesGetHandler,
            ICommandHandler<CreateEntitiesCommand<NewsSubcategory, int>, Task<IEnumerable<NewsSubcategory>>> newsSubcategoriesCreateHandler)
        {
            this.newsProviders = newsProviders;
            this.newsGetHandler = newsGetHandler;
            this.newsCreateHandler = newsCreateHandler;
            this.subcategoriesCreateHandler = subcategoriesCreateHandler;
            this.subcategoriesGetHandler = subcategoriesGetHandler;
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

            IEnumerable<Subcategory> subcategories = await CreateSubcategories(commandSubcategories);
            IEnumerable<News> news = await CreateNews(newsCommands);

            await CreateNewsSubcategories(newsCommands, news, subcategories);
        }

        private async Task<IEnumerable<Subcategory>> CreateSubcategories(IEnumerable<CreateSubcategoryCommand> subcategories)
        {
            IEnumerable<string> subcategoryNames = subcategories.Select(x => x.Name);
            IEnumerable<Subcategory> existingSubcategories = await subcategoriesGetHandler.Handle(new EntitiesByNameQuery<Subcategory, int>(subcategoryNames));
            IEnumerable<string> existingSubcategoryNames = existingSubcategories.Select(x => x.Name);

            IEnumerable<CreateSubcategoryCommand> subcategoriesToCreate = subcategories.Where(x => !existingSubcategoryNames.Contains(x.Name));
            IEnumerable<Subcategory> createdSubcategories = await subcategoriesCreateHandler.Handle(new CreateEntitiesCommand<Subcategory, int>(subcategoriesToCreate));

            List<Subcategory> newsSubcategories = new List<Subcategory>();
            newsSubcategories.AddRange(existingSubcategories);
            newsSubcategories.AddRange(createdSubcategories);

            return await Task.FromResult<IEnumerable<Subcategory>>(newsSubcategories);
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