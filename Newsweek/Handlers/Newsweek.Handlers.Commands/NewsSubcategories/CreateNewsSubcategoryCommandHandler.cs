namespace Newsweek.Handlers.Commands.NewsSubcategories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    
    public class CreateNewsSubcategoryCommandHandler : ICommandHandler<CreateNewsSubcategoryCommand>
    {
        private readonly ICommandHandler<CreateEntitiesCommand<NewsSubcategory, int>, Task<IEnumerable<NewsSubcategory>>> newsSubcategoriesCreateHandler;

        public CreateNewsSubcategoryCommandHandler(ICommandHandler<CreateEntitiesCommand<NewsSubcategory, int>, Task<IEnumerable<NewsSubcategory>>> newsSubcategoriesCreateHandler)
        {
            this.newsSubcategoriesCreateHandler = newsSubcategoriesCreateHandler;
        }

        public async Task Handle(CreateNewsSubcategoryCommand command)
        {
            var newsSubcategoriesCommands = new List<NewsSubcategoryCommand>();

            foreach (var article in command.News)
            {
                NewsCommand newsCommand = command.NewsCommands.FirstOrDefault(x => x.RemoteUrl == article.RemoteUrl);

                if (newsCommand != null)
                {
                    Subcategory subcategory = command.Subcategories.FirstOrDefault(x => x.Name == newsCommand.Subcategory.Name);

                    if (subcategory != null)
                    {
                        newsSubcategoriesCommands.Add(new NewsSubcategoryCommand(article.Id, subcategory.Id));
                    }
                }
            }

            await newsSubcategoriesCreateHandler.Handle(new CreateEntitiesCommand<NewsSubcategory, int>(newsSubcategoriesCommands));
        }
    }
}