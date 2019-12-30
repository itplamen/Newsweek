namespace Newsweek.Handlers.Queries.Common
{
    using System.Linq;

    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Data.Models.Contracts;
    using Newsweek.Handlers.Queries.Contracts;

    public class EntityByNameQueryHandler<TEntity, TKey> : IQueryHandler<EntityByNameQuery<TEntity, TKey>, TEntity>
        where TEntity : BaseModel<TKey>, INameSearchableEntity
    {
        private readonly NewsweekDbContext dbContext;

        public EntityByNameQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TEntity Handle(EntityByNameQuery<TEntity, TKey> query)
        {
            TEntity entity = dbContext.Set<TEntity>()
                .Where(x => x.Name == query.Name)
                .FirstOrDefault();

            return entity;
        }
    }
}