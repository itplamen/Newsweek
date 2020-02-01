namespace Newsweek.Worker.Core.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly IEnumerable<INewsProvider> newsProviders;
        private readonly ICommandHandler<CreateNewsCommand, Task<IEnumerable<News>>> newsCreateHandler;
        private readonly ICommandHandler<CreateSubcategoriesCommand, Task<IEnumerable<Subcategory>>> subcategoriesCreateHandler;

        public NewsTask(
            IEnumerable<INewsProvider> newsProviders,
            ICommandHandler<CreateNewsCommand, Task<IEnumerable<News>>> newsCreateHandler,
            ICommandHandler<CreateSubcategoriesCommand, Task<IEnumerable<Subcategory>>> subcategoriesCreateHandler)
        {
            this.newsProviders = newsProviders;
            this.newsCreateHandler = newsCreateHandler;
            this.subcategoriesCreateHandler = subcategoriesCreateHandler;
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
        }
    }
}