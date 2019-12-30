namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp.Dom;
     
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Worker.Core.Contracts;
    
    public class WorldNewsProvider : BaseNewsProvider
    {
        public WorldNewsProvider(INewsApi newsApi, IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery)
            : base(newsApi, sourceQuery)
        {
            Source = "Reuters";
            CategoryUrls = new string[] { "/news/us", "/places/china", "/subjects/middle-east" };
        }

        protected override IEnumerable<string> GetArticleUrls(IDocument document)
        {
            return document.QuerySelectorAll(".ImageStoryTemplate_image-story-container")?
                .Take(10)?
                .Select(x => x.QuerySelector(".FeedItemHeadline_headline, .FeedItemHeadline_full")?
                    .FirstElementChild?
                    .Attributes["href"]?
                    .Value);
        }

        protected override string GetTitle(IDocument document)
        {
            return document.QuerySelector(".ArticleHeader_headline")?.InnerHtml?.Trim();
        }

        protected override string GetDescription(IDocument document)
        {
            throw new System.NotImplementedException();
        }

        protected override string GetContent(IDocument document)
        {
            IEnumerable<string> content = document.QuerySelectorAll(".StandardArticleBody_body p")?.Select(x => x.OuterHtml);
            
            return string.Join(" ", content);
        }

        protected override string GetMainImageUrl(IDocument document)
        {
            IElement element = document.QuerySelector(".LazyImage_container");
            string src = element?.FirstElementChild?.Attributes["src"]?.Value;

            if (!string.IsNullOrEmpty(src))
            {
                return src.Substring(0, src.LastIndexOf("&"));
            }

            return string.Empty;
        }
    }
}