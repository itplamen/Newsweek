namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Dom;
        
    using Newsweek.Worker.Core.Contracts;
    
    public class ITNewsProvider : BaseNewsProvider
    {
        public ITNewsProvider(INewsApi newsApi)
            : base(newsApi)
        {
            SubcategoryUrls = new string[] { "news" };
        }

        public override string Source => "CIO";

        public override string Category => "IT";

        protected override IEnumerable<string> GetArticleUrls(IDocument document)
        {
            return document.QuerySelectorAll("article.item div.item-image a")?.Select(x => x.Attributes["href"]?.Value);
        }

        protected override string GetTitle(IDocument document)
        {
            return document.QuerySelector("header.entry-header h1.entry-title")?.InnerHtml?.Trim();
        }

        protected override string GetDescription(IDocument document)
        {
            return document.QuerySelector("meta[name=description]")?.Attributes["content"]?.Value;
        }

        protected override string GetContent(IDocument document)
        {
            IEnumerable<string> content = document.QuerySelectorAll("div.entry-content div#link_wrapped_content p")?.Select(x => x.OuterHtml);

            return string.Join(" ", content);
        }

        protected override string GetMainImageUrl(IDocument document)
        {
            return document.QuerySelector("section.layout--right-rail div.post-thumbnail img")?.Attributes["src"]?.Value;
        }

        protected override string GetSubcategory(IDocument document)
        {
            return SubcategoryUrls.First();
        }

        protected override IEnumerable<string> GetTags(IDocument document)
        {
            return Enumerable.Empty<string>();
        }
    }
}