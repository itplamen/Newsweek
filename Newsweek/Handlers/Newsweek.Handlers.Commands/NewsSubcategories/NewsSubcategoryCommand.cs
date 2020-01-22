namespace Newsweek.Handlers.Commands.NewsSubcategories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;

    public class NewsSubcategoryCommand : ICommand, IMapTo<NewsSubcategory>
    {
        public NewsSubcategoryCommand(int newsId, int subcategoryId)
        {
            NewsId = newsId;
            SubcategoryId = subcategoryId;
        }

        public int NewsId { get; set; }

        public int SubcategoryId { get; set; }
    }
}