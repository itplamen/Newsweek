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

    public abstract class BaseNewsProvider : INewsProvider
    {
        private readonly INewsApi newsApi;
        private readonly IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceHandler;

        public BaseNewsProvider(INewsApi newsApi, IQueryHandler<EntityByNameQuery<Source, int>, Source> sourceHandler)
        {
            this.newsApi = newsApi;
            this.sourceHandler = sourceHandler;
        }

        protected string Source { get; set; }

        protected IEnumerable<string> CategoryUrls { get; set; }

        public async Task<IEnumerable<CreateNewsCommand>> Get()
        {
            Source source = sourceHandler.Handle(new EntityByNameQuery<Source, int>(Source));

            var tasks = new List<Task<IEnumerable<CreateNewsCommand>>>();

            foreach (var categoryUrl in CategoryUrls)
            {
                tasks.Add(GetNews(source, categoryUrl));
            }

            var newsCommands = await Task.WhenAll(tasks);

            return newsCommands.SelectMany(news => news)
                .Where(x => x != null);
        }

        private async Task<IEnumerable<CreateNewsCommand>> GetNews(Source source, string categoryUrl)
        {
            IDocument categoryDocument = await newsApi.Get($"{source.Url}/{categoryUrl}");
            IEnumerable<string> articleUrls = GetArticleUrls(categoryDocument).Select(x => SelectArticleUrl(x, source.Url));

            var tasks = new List<Task<CreateNewsCommand>>();

            foreach (var url in articleUrls)
            {
                tasks.Add(BuildArticle(url, source.Id));
            }
            
            return await Task.WhenAll(tasks);
        }

        private async Task<CreateNewsCommand> BuildArticle(string url, int sourceId)
        {
            IDocument document = await newsApi.Get(url);

            if (document != null)
            {
                string title = GetTitle(document);
                string description = GetDescription(document);
                string content = GetContent(document);
                string imageUrl = GetMainImageUrl(document);
                string subcategory = GetSubcategory(document);

                if (IsArticleValid(title, description, content))
                {
                    return new CreateNewsCommand(title, description, content, url, imageUrl, subcategory, sourceId);
                }
            }

            return null;
        }

        private string SelectArticleUrl(string articleUrl, string sourceUrl)
        {
            if (!articleUrl.Contains(sourceUrl))
            {
                return $"{sourceUrl}/{articleUrl}";
            }

            return articleUrl;
        }

        private bool IsArticleValid(string title, string description, string content)
        {
            return !string.IsNullOrEmpty(title) &&
                    !string.IsNullOrEmpty(content) &&
                    !string.IsNullOrEmpty(description);
        }

        protected abstract IEnumerable<string> GetArticleUrls(IDocument document);

        protected abstract string GetTitle(IDocument document);

        protected abstract string GetDescription(IDocument document);

        protected abstract string GetContent(IDocument document);

        protected abstract string GetMainImageUrl(IDocument document);

        protected abstract string GetSubcategory(IDocument document);
    }
}