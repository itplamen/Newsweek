namespace Newsweek.Data.Models
{
    public class NewsSubcategory
    {
        public int NewsId { get; set; }

        public News News { get; set; }

        public int SubcategoryId { get; set; }

        public Subcategory Subcategory { get; set; }
    }
}