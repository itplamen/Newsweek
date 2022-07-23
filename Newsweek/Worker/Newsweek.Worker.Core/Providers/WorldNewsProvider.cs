namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AngleSharp.Dom;

    using Newsweek.Worker.Core.Contracts;
    
    public class WorldNewsProvider : BaseNewsProvider
    {
        public WorldNewsProvider(INewsApi newsApi)
            : base(newsApi)
        {
            SubcategoryUrls = new string[] { "us-canada", "asia", "middle-east" };
        }

        public override string Source => "Al Jazeera";

        public override string Category => "World";

        protected override IEnumerable<string> GetArticleUrls(IDocument document)
        {
            return document.QuerySelectorAll("article.gc--type-post")
                ?.Select(x => x.QuerySelector("a.u-clickable-card__link")
                ?.Attributes["href"]
                ?.Value);
        }

        protected override string GetTitle(IDocument document)
        {
            return document.QuerySelector("header.article-header h1")?.InnerHtml?.Trim();
        }

        protected override string GetDescription(IDocument document)
        {
            return document.QuerySelector("header.article-header p.article__subhead.css-1wt8oh6 em")?.InnerHtml;
        }

        protected override string GetContent(IDocument document)
        {
            IEnumerable<string> content = document.QuerySelectorAll("main#main-content-area div.wysiwyg.wysiwyg--all-content.css-ibbk12 p")?.Select(x => x.OuterHtml);
            
            return string.Join(" ", content);
        }

        protected override string GetMainImageUrl(IDocument document)
        {
            string imgUrl = document.QuerySelector("figure.article-featured-image div.responsive-image")?.FirstElementChild?.Attributes["src"]?.Value;
            string fullUrl = document.QuerySelector("link[rel=canonical]")?.Attributes["href"]?.Value;
            string[] split = fullUrl.Split("/news/");

            return $"{split.First()}/{imgUrl}";
        }

        protected override string GetSubcategory(IDocument document)
        {
            string subcategory = document.QuerySelector("meta[name=primaryTag]")?.Attributes["content"]?.Value;

            if (!string.IsNullOrEmpty(subcategory))
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subcategory.ToLower());
            }

            return string.Empty;
        }
    }
}