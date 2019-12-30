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
    
    public class SportNewsProvider : INewsProvider
    {
        private readonly IEnumerable<string> newsUrls;

        private readonly INewsApi newsApi;
        private readonly IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery;

        public SportNewsProvider(INewsApi newsApi, IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery)
        {
            this.newsApi = newsApi;
            this.sourceQuery = sourceQuery;
            this.newsUrls = new List<string>()
            {
                "/football",
                "/sport/boxing",
                "/sport/tennis"
            };
        }

        public async Task<IEnumerable<CreateNewsCommand>> Get()
        {
            Source source = sourceQuery.Handle(new EntityByNameQuery<Source, int>("talkSPORT"));

            var tasks = new List<Task<IEnumerable<CreateNewsCommand>>>();

            foreach (var url in newsUrls)
            {
                tasks.Add(GetNews(source, url));
            }

            var newsCommands = await Task.WhenAll(tasks);

            return newsCommands.SelectMany(news => news);
        }

        private async Task<IEnumerable<CreateNewsCommand>> GetNews(Source source, string url)
        {
            IDocument document = await newsApi.Get($"{source.Url}/{url}");

            IEnumerable<string> articleUrls = document.QuerySelectorAll("div.sun-row.teaser div.teaser__copy-container a.text-anchor-wrap")?
                .Select(x => x.Attributes["href"]?.Value);

            ICollection<CreateNewsCommand> news = new List<CreateNewsCommand>();

            foreach (var articleUrl in articleUrls)
            {
                IDocument newsDocument = await newsApi.Get(articleUrl);
                string title = newsDocument.QuerySelector("h1.article__headline")?.InnerHtml?.Trim();
                string description = newsDocument.QuerySelector("p.article__content.article__content--intro")?.InnerHtml?.Trim();
                string content = newsDocument.QuerySelector("div.article__content")?.InnerHtml;
                string imageUrl = GetMainImageUrl(newsDocument);

                if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(content))
                {
                    CreateNewsCommand command = new CreateNewsCommand(title, description, content, articleUrl, imageUrl, source.Id);
                    news.Add(command);
                }
            }

            return news;
        }

        private string GetMainImageUrl(IDocument newsDocument)
        {
            IElement element = newsDocument.QuerySelector("div.article__media-img-container.open-gallery a p img");
            string src = element?.Attributes["src"]?.Value;

            if (!string.IsNullOrEmpty(src))
            {
                return src.Substring(0, src.LastIndexOf("?"));
            }

            return string.Empty;
        }
    }
}