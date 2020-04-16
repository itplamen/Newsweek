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
    using DataNews = Data.Models.News;

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
            newsCount.Total = dbContext.Set<DataNews>().Count();
            newsCount.Deleted = dbContext.Set<DataNews>().Count(x => x.IsDeleted);
            newsCount.Active = dbContext.Set<DataNews>().Count(x => !x.IsDeleted);
            newsCount.Today = dbContext.Set<DataNews>().Count(x => x.CreatedOn == DateTime.Today);
            newsCount.Yesterday = dbContext.Set<DataNews>().Count(x => x.CreatedOn == DateTime.Today.AddDays(-1));

            return newsCount;
        }

        private async Task<IEnumerable<NewsByMonthCountResult>> GetNewsByMonthCount(CancellationToken cancellationToken)
        {
            return await dbContext.Set<DataNews>()
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