namespace Newsweek.Handlers.Commands.NewsSubcategories
{
    using System.Collections.Generic;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using DataNews = Data.Models.News;

    public class CreateNewsSubcategoryCommand : ICommand
    {
        public CreateNewsSubcategoryCommand(IEnumerable<DataNews> news, IEnumerable<NewsCommand> newsCommands, IEnumerable<Subcategory> subcategories)
        {
            News = news;
            NewsCommands = newsCommands;
            Subcategories = subcategories;
        }

        public IEnumerable<DataNews> News { get; set; }

        public IEnumerable<NewsCommand> NewsCommands { get; set; }

        public IEnumerable<Subcategory> Subcategories { get; set; }
    }
}