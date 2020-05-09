namespace Newsweek.Worker.Core.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AngleSharp.Dom;

    using MediatR;

    using Newsweek.Data.Models;
    using Newsweek.Data.Models.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Handlers.Commands.Tags;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Worker.Core.Contracts;

    public abstract class BaseNewsProvider : INewsProvider
    {
        private readonly INewsApi newsApi;
        private readonly IMediator mediator;

        public BaseNewsProvider(INewsApi newsApi, IMediator mediator)
        {
            this.newsApi = newsApi;
            this.mediator = mediator;
        }

        protected string Source { get; set; }

        protected string Category { get; set; }

        protected IEnumerable<string> SubcategoryUrls { get; set; }

        public async Task<IEnumerable<NewsCommand>> Get()
        {
            var tasks = new List<Task<IEnumerable<NewsCommand>>>();

            IEnumerable<Source> sources = await GetEntities<Source>(Source);
            IEnumerable<Category> categories = await GetEntities<Category>(Category);

            foreach (var subcategoryUrl in SubcategoryUrls)
            {
                tasks.Add(GetNews(sources.First(), categories.First(), subcategoryUrl));
            }

            var newsCommands = await Task.WhenAll(tasks);

            return newsCommands.SelectMany(news => news).Where(x => x != null);
        }

        protected abstract IEnumerable<string> GetArticleUrls(IDocument document);

        protected abstract string GetTitle(IDocument document);

        protected abstract string GetDescription(IDocument document);

        protected abstract string GetContent(IDocument document);

        protected abstract string GetMainImageUrl(IDocument document);

        protected abstract string GetSubcategory(IDocument document);

        protected IElement RemoveSelectorsFromElement(IElement element, string selectors)
        {
            IHtmlCollection<IElement> elementsToRemove = element?.QuerySelectorAll(selectors);

            if (elementsToRemove != null)
            {
                foreach (var elementToRemove in elementsToRemove)
                {
                    elementToRemove.Remove();
                }
            }

            return element;
        }

        protected virtual IEnumerable<string> GetTags(IDocument document)
        {
            string metaTag = document.QuerySelector("meta[name=news_keywords]")?.OuterHtml;

            if (!string.IsNullOrEmpty(metaTag))
            {
                string[] separators = new string[] { "content=", "<meta name=\"news_keywords", ">", "\"", ",", "(", ")", "/", ";" };

                IEnumerable<string> tags = metaTag.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.Trim().ToLower());

                return tags;
            }

            return Enumerable.Empty<string>();
        }

        private async Task<IEnumerable<TEntity>> GetEntities<TEntity>(string element)
            where TEntity : BaseModel<int>, INameSearchableEntity
        {
            GetEntitiesQuery<TEntity> query = new GetEntitiesQuery<TEntity>()
            {
                Predicate = x => Enumerable.Repeat(element, 1).Contains(x.Name)
            };

            return await mediator.Send(query);
        }

        private async Task<IEnumerable<NewsCommand>> GetNews(Source source, Category category, string subcategoryUrl)
        {
            IDocument subcategoryDocument = await newsApi.Get($"{source.Url}/{subcategoryUrl}");
            IEnumerable<string> articleUrls = GetArticleUrls(subcategoryDocument).Select(x => SelectArticleUrl(x, source.Url));

            var tasks = new List<Task<NewsCommand>>();

            foreach (var url in articleUrls)
            {
                tasks.Add(BuildArticle(url, source.Id, category.Id));
            }
            
            return await Task.WhenAll(tasks);
        }

        private async Task<NewsCommand> BuildArticle(string url, int sourceId, int categoryId)
        {
            IDocument document = await newsApi.Get(url);

            if (document != null)
            {
                string title = GetTitle(document);
                string description = GetDescription(document);
                string mainImageUrl = GetMainImageUrl(document);
                string content = GetContent(document);
 
                if (IsArticleValid(title, description, content))
                {
                    string subcategory = GetSubcategory(document);
                    var subcategoryCommand = new SubcategoryCommand(subcategory, categoryId);
                    IEnumerable<TagCommand> tags = GetTags(document).Select(x => new TagCommand(x));

                    return new NewsCommand()
                    {
                        Title = title, 
                        Description = description, 
                        Content = content, 
                        RemoteUrl = url, 
                        MainImageUrl = mainImageUrl, 
                        SourceId = sourceId,
                        Subcategory = subcategoryCommand,
                        Tags = tags
                    };
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
    }
}