namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp.Dom;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Worker.Core.Contracts;

    public abstract class BaseNewsProvider : INewsProvider
    {
        private readonly INewsApi newsApi;
        private readonly IQueryHandler<EntitiesByNameQuery<Source, int>, Task<IEnumerable<Source>>> sourceHandler;
        private readonly IQueryHandler<EntitiesByNameQuery<Category, int>, Task<IEnumerable<Category>>> categoryHandler;

        public BaseNewsProvider(
            INewsApi newsApi, 
            IQueryHandler<EntitiesByNameQuery<Source, int>, Task<IEnumerable<Source>>> sourceHandler,
            IQueryHandler<EntitiesByNameQuery<Category, int>, Task<IEnumerable<Category>>> categoryHandler)
        {
            this.newsApi = newsApi;
            this.sourceHandler = sourceHandler;
            this.categoryHandler = categoryHandler;
        }

        protected string Source { get; set; }

        protected string Category { get; set; }

        protected IEnumerable<string> SubcategoryUrls { get; set; }

        public async Task<IEnumerable<CreateNewsCommand>> Get()
        {
            var tasks = new List<Task<IEnumerable<CreateNewsCommand>>>();
            var sources = await sourceHandler.Handle(new EntitiesByNameQuery<Source, int>(Enumerable.Repeat(Source, 1)));
            var categories = await categoryHandler.Handle(new EntitiesByNameQuery<Category, int>(Enumerable.Repeat(Category, 1)));
            
            foreach (var subcategoryUrl in SubcategoryUrls)
            {
                tasks.Add(GetNews(sources.First(), categories.First(), subcategoryUrl));
            }

            var newsCommands = await Task.WhenAll(tasks);

            return newsCommands.SelectMany(news => news)
                .Where(x => x != null);
        }

        private async Task<IEnumerable<CreateNewsCommand>> GetNews(Source source, Category category, string subcategoryUrl)
        {
            IDocument subcategoryDocument = await newsApi.Get($"{source.Url}/{subcategoryUrl}");
            IEnumerable<string> articleUrls = GetArticleUrls(subcategoryDocument).Select(x => SelectArticleUrl(x, source.Url));

            var tasks = new List<Task<CreateNewsCommand>>();

            foreach (var url in articleUrls)
            {
                tasks.Add(BuildArticle(url, source.Id, category.Id));
            }
            
            return await Task.WhenAll(tasks);
        }

        private async Task<CreateNewsCommand> BuildArticle(string url, int sourceId, int categoryId)
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
                    var subcategoryCommand = new CreateSubcategoryCommand(subcategory, categoryId);

                    return new CreateNewsCommand(title, description, content, url, imageUrl, subcategoryCommand, sourceId);
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