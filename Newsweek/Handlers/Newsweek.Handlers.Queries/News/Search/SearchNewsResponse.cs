namespace Newsweek.Handlers.Queries.News.Search
{
    using System.Collections.Generic;
 
    public class SearchNewsResponse
    {
        public string Search { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Data.Models.News> News { get; set; }
    }
}