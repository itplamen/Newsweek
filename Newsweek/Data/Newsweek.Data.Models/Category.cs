namespace Newsweek.Data.Models
{
    using System.Collections.Generic;

    using Newsweek.Data.Models.Contracts;

    public class Category : BaseModel<int>, INameSearchableEntity
    {
        public Category()
        {
            News = new HashSet<NewsCategory>();
        }

        public string Name { get; set; }

        public ICollection<NewsCategory> News { get; set; }
    }
}