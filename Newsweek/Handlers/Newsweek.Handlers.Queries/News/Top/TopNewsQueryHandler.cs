namespace Newsweek.Handlers.Queries.News.Top
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;
    
    using Microsoft.EntityFrameworkCore;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data;
    using Newsweek.Data.Models;
    
    public class TopNewsQueryHandler<TViewModel> : IRequestHandler<TopNewsQuery<TViewModel>, IEnumerable<TViewModel>>
        where TViewModel : class
    {
        private readonly NewsweekDbContext dbContext;
 
        public TopNewsQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TViewModel>> Handle(TopNewsQuery<TViewModel> request, CancellationToken cancellationToken)
        {
            IEnumerable<TViewModel> news = await dbContext.Set<Category>()
                .SelectMany(x => x.Subcategories
                    .SelectMany(y => y.News)
                    .OrderByDescending(y => y.Id)
                    .Take(request.Take))
                .To<TViewModel>()
                .ToListAsync(cancellationToken);

            return news;
        }
    }
}