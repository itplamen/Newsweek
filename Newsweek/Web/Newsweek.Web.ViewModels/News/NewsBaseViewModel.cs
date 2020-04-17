namespace Newsweek.Web.ViewModels.News
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Web.ViewModels.Common;

    public class NewsBaseViewModel : BaseViewModel, IMapFrom<Data.Models.News>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string MainImageUrl { get; set; }
    }
}