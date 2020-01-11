namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Dom;
     
    using Newsweek.Data.Models;
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
            return document.QuerySelectorAll("div.ImageStoryTemplate_image-story-container")?
                .Take(10)?
                .Select(x => x.QuerySelector("h2.FeedItemHeadline_headline.FeedItemHeadline_full")?
                    .FirstElementChild?
                    .Attributes["href"]?
                    .Value);
        }

        protected override string GetTitle(IDocument document)
        {
            return document.QuerySelector("h1.ArticleHeader_headline")?.InnerHtml?.Trim();
        }

        protected override string GetDescription(IDocument document)
        {
            return document.QuerySelector("div.StandardArticleBody_body p")?.InnerHtml;
        }

        protected override string GetContent(IDocument document)
        {
            IEnumerable<string> content = document.QuerySelectorAll("div.StandardArticleBody_body p")?.Select(x => x.OuterHtml);
            
            return string.Join(" ", content);
        }

        protected override string GetMainImageUrl(IDocument document)
        {
            IElement element = document.QuerySelector("div.LazyImage_container");
            string src = element?.FirstElementChild?.Attributes["src"]?.Value;

            if (!string.IsNullOrEmpty(src))
            {
                return src.Substring(0, src.LastIndexOf("&"));
            }

            return string.Empty;
        }

        protected override string GetSubcategory(IDocument document)
        {
            string keywords = document.QuerySelector("meta[name=keywords]")?.Attributes["content"]?.Value?.ToLower();

            if (string.IsNullOrEmpty(keywords))
            {
                return string.Empty;
            }

            if (keywords.Contains("china"))
            {
                return "China";
            }

            if (keywords.Contains("middle east"))
            {
                return "Middle East";
            }

            return "USA";
        }
    }
}