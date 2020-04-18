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
        private const int NEWS_PER_PAGE = 9;

        private readonly NewsweekDbContext dbContext;

        public SearchNewsQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SearchNewsResponse> Handle(SearchNewsQuery request, CancellationToken cancellationToken)
        {
            (SearchCriteriaResponse search, Expression<Func<DataNews, bool>> expression) criteria = GetSearchCriteria(request);
            int skip = GetNewsSkipCount(request.Page);

            SearchNewsResponse response = new SearchNewsResponse();
            response.Search = criteria.search;
            response.NewsCount = dbContext.Set<DataNews>().Count(criteria.expression);
            response.PagesCount = (int)Math.Ceiling(response.NewsCount / (decimal)NEWS_PER_PAGE);
            response.CurrentPage = DetermineCurrentPage(request.Page, response.NewsCount);
            response.News = await dbContext.Set<DataNews>()
                .Where(criteria.expression)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(NEWS_PER_PAGE)
                .ToListAsync(cancellationToken);

            return response;
        }

        private (SearchCriteriaResponse, Expression<Func<DataNews, bool>>) GetSearchCriteria(SearchNewsQuery request)
        {
            SearchCriteriaResponse search = new SearchCriteriaResponse();
            Expression<Func<DataNews, bool>> expression = null; ;

            if (!string.IsNullOrWhiteSpace(request.Tag))
            {
                search.Category = request.Tag;
                expression = x => !x.IsDeleted && x.Tags.Any(y => y.Tag.Name == request.Tag.ToLower());
            }
            else
            {
                search.Category = request.Category;
                expression = x => !x.IsDeleted && x.Subcategory.Category.Name == request.Category;

                if (!string.IsNullOrWhiteSpace(request.Subcategory))
                {
                    search.Subcategory= request.Subcategory;
                    expression = x => !x.IsDeleted && 
                        x.Subcategory.Category.Name == request.Category && 
                        x.Subcategory.Name == request.Subcategory;
                }
            }

            return (search, expression);
        }

        private int GetNewsSkipCount(int requestPage)
        {
            if (requestPage > 0 )
            {
                return (requestPage - 1) * NEWS_PER_PAGE;
            }

            return 0;
        }

        private int DetermineCurrentPage(int requestPage, int newsCount)
        {
            if (newsCount > NEWS_PER_PAGE && requestPage == 0)
            {
                return 1;
            }

            return requestPage;
        }
    }
}