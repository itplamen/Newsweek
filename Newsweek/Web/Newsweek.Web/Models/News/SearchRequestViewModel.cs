namespace Newsweek.Web.Models.News
{
    public class SearchRequestViewModel
    {
        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Tag { get; set; }

        public int Take { get; private set; } = 9;
    }
}