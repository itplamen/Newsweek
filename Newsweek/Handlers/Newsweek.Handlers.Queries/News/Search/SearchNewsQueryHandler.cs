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
    using Newsweek.Data.Models;
    
    public class SearchNewsQueryHandler : IRequestHandler<SearchNewsQuery, SearchNewsResponse>
    {
        private readonly NewsweekDbContext dbContext;

        public SearchNewsQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<SearchNewsResponse> Handle(SearchNewsQuery request, CancellationToken cancellationToken)
        {
            (SearchCriteriaResponse search, Expression<Func<News, bool>> expression) criteria = GetSearchCriteria(request);
            int skip = GetNewsSkipCount(request.Page, request.NewsPerPage);

            SearchNewsResponse response = new SearchNewsResponse();
            response.Search = criteria.search;
            response.NewsCount = dbContext.Set<News>().Count(criteria.expression);
            response.PagesCount = (int)Math.Ceiling(response.NewsCount / (decimal)request.NewsPerPage);
            response.CurrentPage = DetermineCurrentPage(request.Page, response.NewsCount, request.NewsPerPage);
            response.News = await dbContext.Set<News>()
                .Where(criteria.expression)
                .OrderByDescending(x => x.Id)
                .Skip(skip)
                .Take(request.NewsPerPage)
                .ToListAsync(cancellationToken);

            return response;
        }

        private (SearchCriteriaResponse, Expression<Func<News, bool>>) GetSearchCriteria(SearchNewsQuery request)
        {
            SearchCriteriaResponse search = new SearchCriteriaResponse();
            Expression<Func<News, bool>> expression = null;

            if (string.IsNullOrWhiteSpace(request.Tag) &&
                string.IsNullOrWhiteSpace(request.Category) &&
                string.IsNullOrWhiteSpace(request.Subcategory))
            {
                expression = x => true;
            }
            else if (!string.IsNullOrWhiteSpace(request.Tag))
            {
                search.Category = request.Tag;
                expression = x => !x.IsDeleted && x.Tags.Any(y => y.Tag.Name.ToLower() == request.Tag.ToLower());
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

        private int GetNewsSkipCount(int requestPage, int newsPerPage)
        {
            if (requestPage > 0 )
            {
                return (requestPage - 1) * newsPerPage;
            }

            return 0;
        }

        private int DetermineCurrentPage(int requestPage, int newsCount, int newsPerPage)
        {
            if (newsCount > newsPerPage && requestPage == 0)
            {
                return 1;
            }

            return requestPage;
        }
    }
}