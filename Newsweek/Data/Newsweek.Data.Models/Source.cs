namespace Newsweek.Data.Models
{
    using System.Collections.Generic;

    using Newsweek.Data.Models.Contracts;

    public class Source : BaseModel<int>, INameSearchableEntity
    {
        public Source()
        {
            News = new HashSet<News>();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public ICollection<News> News { get; set; }
    }
}