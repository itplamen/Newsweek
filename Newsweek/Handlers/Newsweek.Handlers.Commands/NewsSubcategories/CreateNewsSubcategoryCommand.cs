namespace Newsweek.Handlers.Commands.NewsSubcategories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;

    public class CreateNewsSubcategoryCommand : ICommand, IMapTo<NewsSubcategory>
    {
        public int NewsId { get; set; }

        public int SubcategoryId { get; set; }
    }
}