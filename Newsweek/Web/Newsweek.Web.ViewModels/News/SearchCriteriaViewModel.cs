namespace Newsweek.Web.ViewModels.News
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Handlers.Queries.News.Search;

    public class SearchCriteriaViewModel : IMapFrom<SearchCriteriaResponse>
    {
        public string Category { get; set; }

        public string Subcategory { get; set; }
    }
}