namespace Newsweek.Handlers.Queries.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.EntityFrameworkCore;
    
    using Newsweek.Data;
    using Newsweek.Data.Models;
    
    public class GetEntitiesQueryHandler<TEntity> : IRequestHandler<GetEntitiesQuery<TEntity>, IEnumerable<TEntity>>
        where TEntity : BaseModel<int>
    {
        private readonly NewsweekDbContext dbContext;

        public GetEntitiesQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> Handle(GetEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> entities = dbContext.Set<TEntity>();

            if (request.Predicate != null)
            {
                entities = entities.Where(request.Predicate);
            }

            if (request.OrderBy != null)
            {
                entities = request.OrderBy(entities);
            }

            if (request.Take > 0)
            {
                entities = entities.Take(request.Take);
            }

            return await entities.ToListAsync(cancellationToken);
        }
    }
}