using AngleSharp.Dom;
using Newsweek.Data.Models;
using Newsweek.Handlers.Commands.News;
using Newsweek.Handlers.Queries.Contracts;
using Newsweek.Handlers.Queries.Sources;
using Newsweek.Worker.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newsweek.Worker.Core.Providers
{
    public class WorldNewsProvider : INewsProvider
    {
        private readonly INewsApi newsApi;
        private readonly IQueryHandler<SourceByNameQuery, Source> sourceQuery;

        public WorldNewsProvider(INewsApi newsApi, IQueryHandler<SourceByNameQuery, Source> sourceQuery)
        {
            this.newsApi = newsApi;
            this.sourceQuery = sourceQuery;
        }

        public async Task<IEnumerable<CreateNewsCommand>> Get()
        {
            SourceByNameQuery query = new SourceByNameQuery("Euronews");
            Source source = sourceQuery.Handle(query);

            IDocument document = await newsApi.Get($"{source.Url}/tag/world-news");

            IDictionary<string, string> articles = document.QuerySelectorAll("#c-search-articles .m-object--demi")
                .ToDictionary(x => SelectNewsUrl(x, source.Url), y => SelectDescription(y));

            ICollection<CreateNewsCommand> news = new List<CreateNewsCommand>();

            foreach (var article in articles)
            {
                IDocument newsDocument = await newsApi.Get(article.Key);
                string title = newsDocument.QuerySelector(".c-article-title")?.InnerHtml;
                string content = newsDocument.QuerySelector(".c-article-content, .js-article-content, .article__content, .selectionShareable")?.InnerHtml;
                string imageUrl = GetMainImageUrl(newsDocument);

                CreateNewsCommand command = new CreateNewsCommand(title, article.Value, content, article.Key, imageUrl, source.Id);
                news.Add(command);
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
            IElement element = newsDocument.QuerySelector(".media__img__obj, .media-object__img");
            string url = element?.Attributes["src"]?.Value;

            if (string.IsNullOrEmpty(url))
            {
                url = element?.Attributes["data-src"]?.Value;
            }

            return url;
        }
    }
}
