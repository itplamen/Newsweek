namespace Newsweek.Handlers.Queries.News.Search
{
    using MediatR;

    public class SearchNewsQuery : IRequest<SearchNewsResponse>
    {
        public string Category { get; set; }

        public string Subcategory { get; set; }

        public string Tag { get; set; }

        public int Page { get; set; }
    }
}