namespace Newsweek.Handlers.Queries.Common
{
    using System.Linq;

    using Newsweek.Data;
    using Newsweek.Data.Models.Contracts;

    public abstract class GetEntitiesBaseQueryHandler<TEntity>
        where TEntity : class, IAuditInfo, IDeletableEntity
    {
        private readonly NewsweekDbContext dbContext;

        protected GetEntitiesBaseQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TEntity> Handle(GetEntitiesBaseQuery<TEntity> request)
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

            return entities;
        }
    }
}