namespace Newsweek.Handlers.Queries.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Contracts;
    
    public class SelectEntitiesQueryHandler<TEntity, TResult> : IQueryHandler<SelectEntitiesQuery<TEntity, TResult>, IEnumerable<TResult>>
        where TEntity : BaseModel<int>
    {
        private readonly NewsweekDbContext dbContext;

        public SelectEntitiesQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TResult>> Handle(SelectEntitiesQuery<TEntity, TResult> query)
        {
            IQueryable<TEntity> entities = dbContext.Set<TEntity>();

            if (query.Predicate != null)
            {
                entities = entities.Where(query.Predicate);
            }

            if (query.Selector != null)
            {
                return await entities.Select(query.Selector).ToListAsync();
            }

            return await entities.To<TResult>().ToListAsync();
        }
    }
}