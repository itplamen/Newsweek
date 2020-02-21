namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AngleSharp.Dom;
    
    using MediatR;
    
    using Newsweek.Worker.Core.Contracts;
    
    public class EuropeNewsProvider : BaseNewsProvider
    {
        private readonly IEnumerable<string> excludedSubcategories; 

        public EuropeNewsProvider(INewsApi newsApi, IMediator mediator)
            : base(newsApi, mediator)
        {
            Source = "Euronews";
            Category = "Europe";
            SubcategoryUrls = new string[] { "news/europe" };
            excludedSubcategories = new string[] { "no comment", "world", "sport", "world news", "musica", "outdoor", "view" };
        }

        protected override IEnumerable<string> GetArticleUrls(IDocument document)
        {
            return document.QuerySelectorAll("article.m-object--demi")?
                .Where(x => !excludedSubcategories.Contains(x.QuerySelector("span.program-name")?.InnerHtml?.ToLower()))?
                .Select(x => x.QuerySelector("div.m-object__img figure a.media__img__link")?.Attributes["href"]?.Value);
        }

        protected override string GetTitle(IDocument document)
        {
            return document.QuerySelector("h1.c-article-title, h1.c-article-title--h1 span.media__body__link")?.InnerHtml?.Trim();
        }

        protected override string GetDescription(IDocument document)
        {
            return document.QuerySelector("div.c-article-content.js-article-content p")?.InnerHtml;
        }

        protected override string GetContent(IDocument document)
        {
            return document.QuerySelector("div.c-article-content.js-article-content")?.InnerHtml;
        }

        protected override string GetMainImageUrl(IDocument document)
        {
            IElement element = document.QuerySelector("img.media__img__obj");
            string url = element?.Attributes["src"]?.Value;

            if (string.IsNullOrEmpty(url))
            {
                url = element?.Attributes["data-src"]?.Value;
            }

            return url;
        }

        protected override string GetSubcategory(IDocument document)
        {
            string subcategory = document.QuerySelector("a.media__body__cat")?.InnerHtml;

            if (!string.IsNullOrEmpty(subcategory))
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(subcategory.ToLower());
            }

            return string.Empty;
        }
    }
}