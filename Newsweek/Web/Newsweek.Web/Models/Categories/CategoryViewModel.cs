namespace Newsweek.Web.Models.Categories
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }
    }
}