namespace Newsweek.Web.Models.Categories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Web.Models.Common;

    public class CategoryViewModel : BaseViewModel, IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}