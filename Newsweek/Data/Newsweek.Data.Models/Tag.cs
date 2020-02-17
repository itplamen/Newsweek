namespace Newsweek.Data.Models
{
    using Newsweek.Data.Models.Contracts;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag : BaseModel<int>, INameSearchableEntity
    {
        public Tag()
        {
            News = new HashSet<NewsTag>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<NewsTag> News { get; set; }
    }
}