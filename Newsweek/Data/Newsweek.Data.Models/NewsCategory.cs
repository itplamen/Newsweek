namespace Newsweek.Data.Models
{
    public class NewsCategory
    {
        public int NewsId { get; set; }

        public News News { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}