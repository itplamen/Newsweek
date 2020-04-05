namespace Newsweek.Web.Models.News
{
    using System.Collections.Generic;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Handlers.Queries.News.Search;

    public class SearchResponseViewModel : IMapFrom<SearchNewsResponse>
    {
        public string Search { get; set; }

        public int NewsCount { get; set; }

        public IEnumerable<NewsBaseViewModel> News { get; set; }
    }
}