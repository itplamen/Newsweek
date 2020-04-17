namespace Newsweek.Web.ViewModels.News
{
    public class SearchRequestViewModel
    {
        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Tag { get; set; }

        public int Page { get; set; }
    }
}