namespace Newsweek.Data.Models
{
    using System.Collections.Generic;

    using Newsweek.Data.Models.Contracts;

    public class Category : BaseModel<int>, INameSearchableEntity
    {
        public Category()
        {
            Subcategories = new HashSet<Subcategory>();
        }

        public string Name { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; }
    }
}