namespace Newsweek.Handlers.Queries.Common
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
    
    public class SelectEntitiesQueryHandler<TEntity, TResult> : IRequestHandler<SelectEntitiesQuery<TEntity, TResult>, IEnumerable<TResult>>
        where TEntity : BaseModel<int>
    {
        private readonly NewsweekDbContext dbContext;

        public SelectEntitiesQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TResult>> Handle(SelectEntitiesQuery<TEntity, TResult> request, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> entities = dbContext.Set<TEntity>();

            if (request.Predicate != null)
            {
                entities = entities.Where(request.Predicate);
            }

            if (request.Selector != null)
            {
                return await entities.Select(request.Selector).ToListAsync();
            }

            return await entities.To<TResult>()
                .ToListAsync(cancellationToken);
        }
    }
}