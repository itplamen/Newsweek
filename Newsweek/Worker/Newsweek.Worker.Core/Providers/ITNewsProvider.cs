namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Dom;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Worker.Core.Contracts;
    
    public class ITNewsProvider : BaseNewsProvider
    {
        public ITNewsProvider(INewsApi newsApi, IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery)
            : base(newsApi, sourceQuery)
        {
            Source = "ITworld";
            CategoryUrls = new string[] { "news" };
        }

        protected override IEnumerable<string> GetArticleUrls(IDocument document)
        {
            return document.QuerySelectorAll("div.river-well.article div.post-cont h3 a")?
                .Select(x => x.Attributes["href"]?.Value);
        }

        protected override string GetTitle(IDocument document)
        {
            return document.QuerySelector("h1[itemprop=headline]")?.InnerHtml?.Trim();
        }

        protected override string GetDescription(IDocument document)
        {
            return document.QuerySelector("h3[itemprop=description]")?.InnerHtml?.Trim();
        }

        protected override string GetContent(IDocument document)
        {
            return document.QuerySelector("div#drr-container")?.InnerHtml;
        }

        protected override string GetMainImageUrl(IDocument document)
        {
            return document.QuerySelector("div.lede-container figure[itemprop=image] img")?.Attributes["data-original"]?.Value;
        }

        protected override string GetSubcategory(IDocument document)
        {
            return document.QuerySelector("nav.breadcrumbs.horiz ul li:nth-child(2) a.edition-link-url span")?.InnerHtml;
        }
    }
}