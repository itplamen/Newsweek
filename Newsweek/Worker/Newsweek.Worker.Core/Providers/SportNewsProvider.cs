namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AngleSharp.Dom;
    
    using MediatR;
    
    using Newsweek.Worker.Core.Contracts;
    
    public class SportNewsProvider : BaseNewsProvider
    {
        public SportNewsProvider(INewsApi newsApi, IMediator mediator)
            : base(newsApi, mediator)
        {
            Source = "talkSPORT";
            Category = "Sport";
            SubcategoryUrls = new string[] { "/football", "/sport/boxing", "/sport/tennis" };
        }

        protected override IEnumerable<string> GetArticleUrls(IDocument document)
        {
            return document.QuerySelectorAll("div.sun-row.teaser div.teaser__copy-container a.text-anchor-wrap")?
                .Select(x => x.Attributes["href"]?.Value);
        }

        protected override string GetTitle(IDocument document)
        {
            return document.QuerySelector("h1.article__headline")?.InnerHtml?.Trim();
        }

        protected override string GetDescription(IDocument document)
        {
            return document.QuerySelector("p.article__content.article__content--intro")?.TextContent;
        }

        protected override string GetContent(IDocument document)
        {
            IElement content = document.QuerySelector("div.article__content");
            content = RemoveSelectorsFromElement(content, @"figure.article__media:first-of-type, 
                div.article__credit, div.article__gallery-count.theme__background-color-rgba-80, 
                figcaption.article__media-caption");

            return content?.InnerHtml;
        }

        protected override string GetMainImageUrl(IDocument document)
        {
            IElement element = document.QuerySelector("div.article__media-img-container.open-gallery a p img");
            string src = element?.Attributes["src"]?.Value;

            if (!string.IsNullOrEmpty(src))
            {
                return src.Substring(0, src.LastIndexOf("?"));
            }

            return string.Empty;
        }

        protected override string GetSubcategory(IDocument document)
        {
            string url = document.QuerySelector("link[rel=canonical]")?.Attributes["href"]?.Value?.ToLower();

            foreach (var categoryUrl in SubcategoryUrls)
            {
                if (url.Contains(categoryUrl))
                {
                    string subcategory = categoryUrl.Split("/").Last();

                    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subcategory);
                }
            }

            return string.Empty;
        }

        protected override IEnumerable<string> GetTags(IDocument document)
        {
            IEnumerable<string> tags = document.QuerySelectorAll("li.tags__item.theme__after-color")?
                 .SelectMany(x => x.Children)
                 .Select(x => x.InnerHtml.Trim().ToLower());

            return tags;
        }
    }
}