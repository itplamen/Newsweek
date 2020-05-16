namespace Newsweek.Worker.Core.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MediatR;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.NewsTags;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Handlers.Commands.Tags;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly IMediator mediator;
        private readonly IEnumerable<INewsProvider> newsProviders;
        
        public NewsTask(IMediator mediator, IEnumerable<INewsProvider> newsProviders)
        {
            this.mediator = mediator;
            this.newsProviders = newsProviders;
        }

        public async Task DoWork()
        {
            IEnumerable<NewsCommand> newsCommands = await GetNewsCommands();

            IEnumerable<SubcategoryCommand> subcategoryCommands = newsCommands.Select(x => x.Subcategory)
                .GroupBy(x => x.Name)
                .Select(x => x.First());

            IEnumerable<Subcategory> subcategories = await mediator.Send(new CreateSubcategoriesCommand(subcategoryCommands));

            foreach (var subcategory in subcategories)
            {
                IEnumerable<NewsCommand> newsCommandsToSet = newsCommands.Where(x => x.Subcategory.Name == subcategory.Name);

                foreach (var command in newsCommandsToSet)
                {
                    command.SubcategoryId = subcategory.Id;
                }
            }

            IEnumerable<News> news = await mediator.Send(new CreateNewsCommand(newsCommands));
            IEnumerable<Tag> tags = await mediator.Send(new CreateTagsCommand(newsCommands.SelectMany(x => x.Tags)));
            await mediator.Send(new CreateNewsTagsCommand(newsCommands, news, tags));
        }

        private async Task<IEnumerable<NewsCommand>> GetNewsCommands()
        {
            var sources = await mediator.Send(new GetEntitiesQuery<Source>());
            var categories = await mediator.Send(new GetEntitiesQuery<Category>());

            var newsCommands = new List<NewsCommand>();

            foreach (var newsProvider in newsProviders)
            {
                Source source = sources.FirstOrDefault(x => x.Name == newsProvider.Source);
                Category category = categories.FirstOrDefault(x => x.Name == newsProvider.Category);

                IEnumerable<NewsCommand> commands = await newsProvider.Get(source, category);
                newsCommands.AddRange(commands);
            }

            return newsCommands.GroupBy(x => x.RemoteUrl)
                .Where(x => x.Count() == 1)
                .SelectMany(x => x);
        } 
    }
}