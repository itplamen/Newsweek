namespace Newsweek.Data.Models
{
    using System.Collections.Generic;

    public class Tag : BaseModel<int>
    {
        public Tag()
        {
            News = new HashSet<NewsTag>();
        }

        public string Name { get; set; }

        public ICollection<NewsTag> News { get; set; }
    }
}