namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Subcategory : BaseModel<int>
    {
        public Subcategory()
        {
            News = new HashSet<NewsSubcategory>();
        }

        [Required]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<NewsSubcategory> News { get; set; }
    }
}
