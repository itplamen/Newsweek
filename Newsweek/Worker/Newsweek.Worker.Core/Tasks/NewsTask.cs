namespace Newsweek.Worker.Core.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Handlers.Commands.Tags;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IEnumerable<INewsProvider> newsProviders;
        
        public NewsTask(ICommandDispatcher commandDispatcher, IEnumerable<INewsProvider> newsProviders)
        {
            this.newsProviders = newsProviders;
            this.commandDispatcher = commandDispatcher;
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

            IEnumerable<SubcategoryCommand> subcategoryCommands = newsCommands.Select(x => x.Subcategory)
                .GroupBy(x => x.Name)
                .Select(x => x.First());

            var subcategories = await commandDispatcher
                .Dispatch<CreateSubcategoriesCommand, IEnumerable<Subcategory>>(new CreateSubcategoriesCommand(subcategoryCommands));

            foreach (var subcategory in subcategories)
            {
                IEnumerable<NewsCommand> newsCommandsToSet = newsCommands.Where(x => x.Subcategory.Name == subcategory.Name);

                foreach (var command in newsCommandsToSet)
                {
                    command.SubcategoryId = subcategory.Id;
                }
            }

            await commandDispatcher.Dispatch(new CreateNewsCommand(newsCommands));
            await commandDispatcher.Dispatch(new CreateTagsCommand(newsCommands.SelectMany(x => x.Tags)));
        }
    }
}