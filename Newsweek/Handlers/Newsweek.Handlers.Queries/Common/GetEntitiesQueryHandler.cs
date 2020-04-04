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
    using Newsweek.Data.Models.Contracts;

    public class GetEntitiesQueryHandler<TEntity> : GetEntitiesBaseQueryHandler<TEntity>, IRequestHandler<GetEntitiesQuery<TEntity>, IEnumerable<TEntity>>
        where TEntity : class, IAuditInfo, IDeletableEntity
    {
        public GetEntitiesQueryHandler(NewsweekDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<TEntity>> Handle(GetEntitiesQuery<TEntity> request, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> entities = base.Handle(request);

            return await entities.ToListAsync(cancellationToken);
        }
    }

    public class GetEntitiesQueryHandler<TEntity, TResult> : GetEntitiesBaseQueryHandler<TEntity>, IRequestHandler<GetEntitiesQuery<TEntity, TResult>, IEnumerable<TResult>>
        where TEntity : class, IAuditInfo, IDeletableEntity
    {
        public GetEntitiesQueryHandler(NewsweekDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IEnumerable<TResult>> Handle(GetEntitiesQuery<TEntity, TResult> request, CancellationToken cancellationToken)
        {
            IQueryable<TEntity> entities = base.Handle(request);

            if (request.Selector != null)
            {
                return await entities.Select(request.Selector).ToListAsync(cancellationToken);
            }

            return await entities.To<TResult>().ToListAsync(cancellationToken);
        }
    }
}