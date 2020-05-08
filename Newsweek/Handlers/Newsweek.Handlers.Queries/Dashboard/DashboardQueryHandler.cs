namespace Newsweek.Handlers.Queries.Dashboard
{
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MediatR;

    using Microsoft.EntityFrameworkCore;
    
    using Newsweek.Data;
    using Newsweek.Data.Models;

    public class DashboardQueryHandler : IRequestHandler<DashboardQuery, DashboardResult>
    {
        private readonly NewsweekDbContext dbContext;

        public DashboardQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DashboardResult> Handle(DashboardQuery request, CancellationToken cancellationToken)
        {
            var dashboardResult = new DashboardResult();
            dashboardResult.NewsCount = GetNewsCount();
            dashboardResult.NewsByMonthCount = await GetNewsByMonthCount(cancellationToken);

            return dashboardResult;
        }

        private NewsCountResult GetNewsCount()
        {
            var newsCount = new NewsCountResult();
            newsCount.Total = dbContext.Set<News>().Count();
            newsCount.Deleted = dbContext.Set<News>().Count(x => x.IsDeleted);
            newsCount.Active = dbContext.Set<News>().Count(x => !x.IsDeleted);
            newsCount.Today = dbContext.Set<News>().Count(x => x.CreatedOn == DateTime.Today);
            newsCount.Yesterday = dbContext.Set<News>().Count(x => x.CreatedOn == DateTime.Today.AddDays(-1));

            return newsCount;
        }

        private async Task<IEnumerable<NewsByMonthCountResult>> GetNewsByMonthCount(CancellationToken cancellationToken)
        {
            return await dbContext.Set<News>()
                .GroupBy(x => x.CreatedOn.Month)
                .Select(x => new NewsByMonthCountResult()
                {
                    Month = x.Key,
                    NewsCount = x.Count()
                })
                .ToListAsync(cancellationToken);
        }
    }
}