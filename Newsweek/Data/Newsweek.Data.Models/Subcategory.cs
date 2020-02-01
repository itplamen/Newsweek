namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Data.Models.Contracts;

    public class Subcategory : BaseModel<int>, INameSearchableEntity
    {
        public Subcategory()
        {
            News = new HashSet<News>();
        }

        [Required]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<News> News { get; set; }
    }
}
