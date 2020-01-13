namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp.Dom;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Worker.Core.Contracts;
    
    public class SportNewsProvider : BaseNewsProvider
    {
        public SportNewsProvider(
            INewsApi newsApi, 
            IQueryHandler<EntitiesByNameQuery<Source, int>, Task<IEnumerable<Source>>> sourceHandler,
            IQueryHandler<EntitiesByNameQuery<Category, int>, Task<IEnumerable<Category>>> categoryHandler)
            : base(newsApi, sourceHandler, categoryHandler)
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
            return document.QuerySelector("div.article__content")?.InnerHtml;
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
    }
}