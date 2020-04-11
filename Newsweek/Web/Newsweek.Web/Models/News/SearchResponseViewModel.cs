namespace Newsweek.Web.Models.News
{
    using System.Collections.Generic;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Handlers.Queries.News.Search;

    public class SearchResponseViewModel : IMapFrom<SearchNewsResponse>
    {
        public SearchCriteriaViewModel Search { get; set; }

        public int NewsCount { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int PreviousPage { get; set; }

        public int NextPage { get; set; }

        public IEnumerable<NewsBaseViewModel> News { get; set; }
    }
}