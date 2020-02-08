namespace Newsweek.Handlers.Queries.News
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data;
    using Newsweek.Handlers.Queries.Contracts;

    public class TopNewsQueryHandler<TViewModel> : IQueryHandler<IEnumerable<TViewModel>>
        where TViewModel : class
    {
        private const int NEWS_FOR_SOURCE = 5;

        private readonly NewsweekDbContext dbContext;
 
        public TopNewsQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TViewModel>> Handle()
        {
            IEnumerable<TViewModel> news = await dbContext.Set<Data.Models.News>()
                .Select(x => x.Source)
                .Distinct()
                .SelectMany(x => x.News.Take(NEWS_FOR_SOURCE))
                .To<TViewModel>()
                .ToListAsync();

            return news;
        }
    }
}