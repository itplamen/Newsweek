namespace Newsweek.Web.ViewModels.News
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Web.ViewModels.Common;
    using Newsweek.Web.ViewModels.Subcategories;

    public class NewsBaseViewModel : BaseViewModel, IMapFrom<News>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string MainImageUrl { get; set; }

        public SubcategoryViewModel Subcategory { get; set; }
    }
}