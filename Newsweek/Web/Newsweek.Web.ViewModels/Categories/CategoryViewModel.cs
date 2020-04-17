namespace Newsweek.Web.ViewModels.Categories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Web.ViewModels.Common;

    public class CategoryViewModel : BaseViewModel, IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}