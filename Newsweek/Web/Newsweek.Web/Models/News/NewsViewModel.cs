namespace Newsweek.Web.Models.News
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Web.Models.Sources;
    using Newsweek.Web.Models.Subcategories;

    public class NewsViewModel : IMapFrom<Data.Models.News>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string RemoteUrl { get; set; }

        public string MainImageUrl { get; set; }

        public SourceViewModel Source { get; set; }

        public SubcategoryViewModel Subcategory { get; set; }
    }
}