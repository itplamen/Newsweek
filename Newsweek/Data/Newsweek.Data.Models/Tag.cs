namespace Newsweek.Data.Models
{
    using Newsweek.Data.Models.Contracts;

    using System.Collections.Generic;

    public class Tag : BaseModel<int>, INameSearchableEntity
    {
        public Tag()
        {
            News = new HashSet<NewsTag>();
        }

        public string Name { get; set; }

        public ICollection<NewsTag> News { get; set; }
    }
}