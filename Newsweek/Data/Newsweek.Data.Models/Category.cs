namespace Newsweek.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newsweek.Data.Models.Contracts;

    public class Category : BaseModel<int>, INameSearchableEntity
    {
        public Category()
        {
            Subcategories = new HashSet<Subcategory>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; }
    }
}