namespace Newsweek.Web.ViewModels.Subcategories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Web.ViewModels.Categories;
    using Newsweek.Web.ViewModels.Common;

    public class SubcategoryViewModel : BaseViewModel, IMapFrom<Subcategory>
    {
        public string Name { get; set; }

        public CategoryViewModel Category { get; set; }
    }
}