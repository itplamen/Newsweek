namespace Newsweek.Web.Models.Subcategories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Web.Models.Categories;
    using Newsweek.Web.Models.Common;

    public class SubcategoryViewModel : BaseViewModel, IMapFrom<Subcategory>
    {
        public string Name { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}