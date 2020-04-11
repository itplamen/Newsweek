namespace Newsweek.Handlers.Queries.News.Search
{
    using System.Collections.Generic;
 
    public class SearchNewsResponse
    {
        public SearchCriteriaResponse Search { get; set; }

        public int NewsCount { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int PreviousPage => CurrentPage == 1 ? 1 : CurrentPage - 1;

        public int NextPage => CurrentPage == PagesCount ? PagesCount : CurrentPage + 1;

        public IEnumerable<Data.Models.News> News { get; set; }
    }
}