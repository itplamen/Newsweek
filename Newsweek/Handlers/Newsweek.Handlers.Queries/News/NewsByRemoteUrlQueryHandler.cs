namespace Newsweek.Handlers.Queries.News
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    
    using Newsweek.Data;
    using Newsweek.Handlers.Queries.Contracts;
    using DataNews = Data.Models.News;

    public class NewsByRemoteUrlQueryHandler : IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<DataNews>>>
    {
        private readonly NewsweekDbContext dbContext;

        public NewsByRemoteUrlQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<DataNews>> Handle(NewsByRemoteUrlQuery query)
        {
            IEnumerable<DataNews> news = await dbContext.Set<DataNews>()
                .Where(x => query.RemoteUrls.Contains(x.RemoteUrl))
                .ToListAsync();

            return news;
        }
    }
}