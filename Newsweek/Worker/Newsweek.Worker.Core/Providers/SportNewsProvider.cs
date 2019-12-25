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
    public class SportNewsProvider : INewsProvider
    {
        private readonly INewsApi newsApi;
        private readonly IQueryHandler<SourceByNameQuery, Source> sourceQuery;

        public SportNewsProvider(INewsApi newsApi, IQueryHandler<SourceByNameQuery, Source> sourceQuery)
        {
            this.newsApi = newsApi;
            this.sourceQuery = sourceQuery;
        }

        public async Task<IEnumerable<CreateNewsCommand>> Get()
        {
            SourceByNameQuery query = new SourceByNameQuery("Sky Sports");
            Source source = sourceQuery.Handle(query);

            IDocument document = await newsApi.Get($"{source.Url}/news-wire");

            IEnumerable<KeyValuePair<string, string>> articles = document.QuerySelectorAll(".news-list__item--show-thumb-bp30")
                .ToDictionary(x => SelectNewsUrl(x, source.Url), y => SelectDescription(y))
                .Where(x => !string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.Value));

            ICollection<CreateNewsCommand> news = new List<CreateNewsCommand>();

            foreach (var article in articles)
            {
                IDocument newsDocument = await newsApi.Get(article.Key);
                string title = newsDocument.QuerySelector(".sdc-article-header__long-title, .article__long-title")?.InnerHtml;
                string content = newsDocument.QuerySelector(".sdc-article-body--lead, .article__body--lead")?.InnerHtml;
                string imageUrl = GetMainImageUrl(newsDocument);

                CreateNewsCommand command = new CreateNewsCommand(title, article.Value, content, article.Key, imageUrl, source.Id);
                news.Add(command);
            }

            return news;
        }

        private string SelectNewsUrl(IElement newsElement, string baseUrl)
        {
            IElement imgElement = newsElement.QuerySelector(".news-list__headline-link");
            string newsUrl = imgElement.Attributes["href"].Value;

            if (newsUrl.Contains(baseUrl))
            {
                return newsUrl;
            }

            return string.Empty;
        }

        private string SelectDescription(IElement newsElement)
        {
            return newsElement.QuerySelector(".news-list__snippet")?.InnerHtml;
        }

        private string GetMainImageUrl(IDocument newsDocument)
        {
            IElement element = newsDocument.QuerySelector(".sdc-article-image__item");
            string url = element?.Attributes["src"]?.Value;

            if (string.IsNullOrEmpty(url))
            {
                url = element?.Attributes["data-src"]?.Value;
            }

            return url;
        }
    }
}
