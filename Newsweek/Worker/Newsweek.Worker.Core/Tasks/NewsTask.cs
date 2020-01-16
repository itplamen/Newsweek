namespace Newsweek.Worker.Core.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Handlers.Queries.News;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly IEnumerable<INewsProvider> newsProviders;
        private readonly ICommandHandler<CreateNewsCommand> newsCreateHandler;
        private readonly IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<Subcategory, int>> subcategoriesCreateHandler;
        private readonly IQueryHandler<EntitiesByNameQuery<Subcategory, int>, Task<IEnumerable<Subcategory>>> subcategoriesGetHandler;

        public NewsTask(
            IEnumerable<INewsProvider> newsProviders, 
            ICommandHandler<CreateNewsCommand> newsCreateHandler,
            IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler,
            ICommandHandler<CreateEntitiesCommand<Subcategory, int>> subcategoriesCreateHandler,
            IQueryHandler<EntitiesByNameQuery<Subcategory, int>, Task<IEnumerable<Subcategory>>> subcategoriesGetHandler)
        {
            this.newsProviders = newsProviders;
            this.newsCreateHandler = newsCreateHandler;
            this.newsGetHandler = newsGetHandler;
            this.subcategoriesCreateHandler = subcategoriesCreateHandler;
            this.subcategoriesGetHandler = subcategoriesGetHandler;
        }

        public async Task DoWork()
        {
            List<CreateNewsCommand> newsCommands = new List<CreateNewsCommand>();

            foreach (var newsProvider in newsProviders)
            {
                IEnumerable<CreateNewsCommand> commands = await newsProvider.Get();
                newsCommands.AddRange(commands);
            }

            IEnumerable<SubcategoryCreateCommand> subcategories = newsCommands.Select(x => x.Subcategory)
                .GroupBy(x => x.Name)
                .Select(x => x.First());

            await CreateSubcategories(subcategories);

            IEnumerable<string> urls = newsCommands.Select(x => x.RemoteUrl);
            IEnumerable<News> news = await newsGetHandler.Handle(new NewsByRemoteUrlQuery(urls));

            foreach (var command in newsCommands)
            {
                if (news == null || !news.Any(x => x.RemoteUrl == command.RemoteUrl))
                {
                    newsCreateHandler.Handle(command);
                }
            }
        }

        private async Task CreateSubcategories(IEnumerable<SubcategoryCreateCommand> subcategories)
        {
            IEnumerable<string> subcategoryNames = subcategories.Select(x => x.Name);
            IEnumerable<Subcategory> existingSubcategories = await subcategoriesGetHandler.Handle(new EntitiesByNameQuery<Subcategory, int>(subcategoryNames));
            IEnumerable<string> existingSubcategoryNames = existingSubcategories.Select(x => x.Name);

            IEnumerable<SubcategoryCreateCommand> subcategoriesToCreate = subcategories.Where(x => !existingSubcategoryNames.Contains(x.Name));

            await subcategoriesCreateHandler.Handle(new CreateEntitiesCommand<Subcategory, int>(subcategoriesToCreate));
        }
    }
}