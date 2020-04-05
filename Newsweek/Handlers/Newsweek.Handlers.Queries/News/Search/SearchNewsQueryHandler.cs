namespace Newsweek.Handlers.Queries.News.Search
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.EntityFrameworkCore;

    using Newsweek.Data;
    using DataNews = Data.Models.News;
    
    public class SearchNewsQueryHandler : IRequestHandler<SearchNewsQuery, SearchNewsResponse>
    {
        private const int TAKE_NEWS = 9;

        private readonly NewsweekDbContext dbContext;

        public SearchNewsQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SearchNewsResponse> Handle(SearchNewsQuery request, CancellationToken cancellationToken)
        {
            (string search, Expression<Func<DataNews, bool>> expression) searchData = GetSearch(request);

            SearchNewsResponse response = new SearchNewsResponse();
            response.Search = searchData.search;
            response.TotalCount = dbContext.Set<DataNews>().Count(searchData.expression);
            response.News = await dbContext.Set<DataNews>().Where(searchData.expression).Take(TAKE_NEWS).ToListAsync(cancellationToken);

            return response;
        }

        private (string, Expression<Func<DataNews, bool>>) GetSearch(SearchNewsQuery request)
        {
            string search = string.Empty;
            Expression<Func<DataNews, bool>> expression = null; ;

            if (!string.IsNullOrWhiteSpace(request.Tag))
            {
                search = request.Tag;
                expression = x => !x.IsDeleted && x.Tags.Any(y => y.Tag.Name == request.Tag.ToLower());
            }
            else
            {
                search = $"{request.Category}";
                expression = x => !x.IsDeleted && x.Subcategory.Category.Name == request.Category;

                if (!string.IsNullOrWhiteSpace(request.Subcategory))
                {
                    search += $"/{request.Subcategory}";
                    expression = x => !x.IsDeleted && 
                        x.Subcategory.Category.Name == request.Category && 
                        x.Subcategory.Name == request.Subcategory;
                }
            }

            return (search, expression);
        }
    }
}