namespace Newsweek.Data.Models
{
    using System.Collections.Generic;

    public class Category : BaseModel<int>
    {
        public Category()
        {
            News = new HashSet<NewsCategory>();
        }

        public string Name { get; set; }

        public ICollection<NewsCategory> News { get; set; }
    }
}