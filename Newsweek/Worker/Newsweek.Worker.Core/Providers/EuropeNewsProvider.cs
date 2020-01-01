namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Dom;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Worker.Core.Contracts;
    
    public class EuropeNewsProvider : BaseNewsProvider
    {

        public EuropeNewsProvider(INewsApi newsApi, IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery)
            : base(newsApi, sourceQuery)
        {
            Source = "Euronews";
            CategoryUrls = new string[] { "news/europe" };
        }

        protected override IEnumerable<string> GetArticleUrls(IDocument document)
        {
            return document.QuerySelectorAll("article.m-object--demi")?
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
    }
}