namespace Newsweek.Handlers.Queries.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Data.Models.Contracts;
    using Newsweek.Handlers.Queries.Contracts;

    public class EntitiesByNameQueryHandler<TEntity, TKey> : IQueryHandler<EntitiesByNameQuery<TEntity, TKey>, IEnumerable<TEntity>>
        where TEntity : BaseModel<TKey>, INameSearchableEntity
    {
        private readonly NewsweekDbContext dbContext;

        public EntitiesByNameQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> Handle(EntitiesByNameQuery<TEntity, TKey> query)
        {
            IEnumerable<TEntity> entities = await dbContext.Set<TEntity>()
                .Where(x => query.Names.Contains(x.Name))
                .ToListAsync();

            return entities;
        }
    }
}