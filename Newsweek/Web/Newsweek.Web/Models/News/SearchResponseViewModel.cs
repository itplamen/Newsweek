namespace Newsweek.Web.Models.News
{
    using System.Collections.Generic;

    public class SearchResponseViewModel
    {
        public string Search { get; set; }

        public IEnumerable<NewsBaseViewModel> News { get; set; }
    }
}