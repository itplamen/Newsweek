namespace Newsweek.Web.Models.Subcategories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Web.Models.Categories;

    public class SubcategoryViewModel : IMapFrom<Subcategory>
    {
        public string Name { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}