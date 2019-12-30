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
    
    public class WorldNewsProvider : INewsProvider
    {
        private readonly IEnumerable<string> newsUrls;

        private readonly INewsApi newsApi;
        private readonly IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery;

        public WorldNewsProvider(INewsApi newsApi, IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceQuery)
        {
            this.newsApi = newsApi;
            this.sourceQuery = sourceQuery;
            this.newsUrls = new List<string>()
            {
                "/news/us",
                "/places/china",
                "/subjects/middle-east"
            };
        }

        public async Task<IEnumerable<CreateNewsCommand>> Get()
        {
            Source source = sourceQuery.Handle(new EntityByNameQuery<Source, int>("Reuters"));

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

            IDictionary<string, string> articles = document.QuerySelectorAll(".ImageStoryTemplate_image-story-container")
                .Take(10)
                .ToDictionary(x => SelectNewsUrl(x), y => SelectDescription(y));

            ICollection<CreateNewsCommand> news = new List<CreateNewsCommand>();

            foreach (var article in articles)
            {
                IDocument newsDocument = await newsApi.Get(article.Key);
                string title = newsDocument.QuerySelector(".ArticleHeader_headline")?.InnerHtml?.Trim();
                IEnumerable<string> contents = newsDocument.QuerySelectorAll(".StandardArticleBody_body p")?.Select(x => x.OuterHtml);
                string content = string.Join(" ", contents);
                string imageUrl = GetMainImageUrl(newsDocument);

                if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(content))
                {
                    CreateNewsCommand command = new CreateNewsCommand(title, article.Value, content, article.Key, imageUrl, source.Id);
                    news.Add(command);
                }
            }

            return news;
        }

        private string SelectNewsUrl(IElement newsElement)
        {
            return newsElement.QuerySelector(".FeedItemHeadline_headline, .FeedItemHeadline_full")?
                .FirstElementChild?
                .Attributes["href"]?
                .Value;
        }

        private string SelectDescription(IElement newsElement)
        {
            return newsElement.QuerySelector(".FeedItemLede_lede")?.InnerHtml;
        }

        private string GetMainImageUrl(IDocument newsDocument)
        {
            IElement element = newsDocument.QuerySelector(".LazyImage_container");
            string src = element?.FirstElementChild?.Attributes["src"]?.Value;

            if (!string.IsNullOrEmpty(src))
            {
                return src.Substring(0, src.LastIndexOf("&"));
            }

            return string.Empty;
        }
    }
}