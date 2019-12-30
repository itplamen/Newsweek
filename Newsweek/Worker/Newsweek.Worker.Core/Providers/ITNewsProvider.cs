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
    
    public class ITNewsProvider : INewsProvider
    {
        private readonly INewsApi newsApi;
        private readonly IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery;

        public ITNewsProvider(INewsApi newsApi, IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery)
        {
            this.newsApi = newsApi;
            this.sourceQuery = sourceQuery;
        }

        public async Task<IEnumerable<CreateNewsCommand>> Get()
        {
            Source source = sourceQuery.Handle(new EntityByNameQuery<Source, int>("ITworld"));

            IDocument document = await newsApi.Get($"{source.Url}/news");

            IEnumerable<string> articleUrls = document.QuerySelectorAll("div.river-well.article div.post-cont h3 a")?
                .Select(x => x.Attributes["href"]?.Value);

            ICollection<CreateNewsCommand> news = new List<CreateNewsCommand>();

            foreach (var articleUrl in articleUrls)
            {
                IDocument newsDocument = await newsApi.Get($"{source.Url}/{articleUrl}");
                string title = newsDocument.QuerySelector("h1[itemprop=headline]")?.InnerHtml?.Trim();
                string description = newsDocument.QuerySelector("h3[itemprop=description]")?.InnerHtml?.Trim();
                string content = newsDocument.QuerySelector("div#drr-container")?.InnerHtml;
                string imageUrl = GetMainImageUrl(newsDocument);

                if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(content))
                {
                    CreateNewsCommand command = new CreateNewsCommand(title, description, content, articleUrl, imageUrl, source.Id);
                    news.Add(command);
                }
            }

            return news;
        }

        private string SelectNewsUrl(IElement newsElement, string baseUrl)
        {
            IElement imgElement = newsElement.QuerySelector(".m-object__img .media__img__link");
            string newsUrl = $"{baseUrl}{imgElement.Attributes["href"].Value}";

            return newsUrl;
        }

        private string SelectDescription(IElement newsElement)
        {
            return newsElement.QuerySelector(".m-object__description__link p")?.InnerHtml;
        }

        private string GetMainImageUrl(IDocument newsDocument)
        {
            IElement element = newsDocument.QuerySelector("div.lede-container figure[itemprop=image] img");
            string url = element?.Attributes["data-original"]?.Value;

            return url;
        }
    }
}