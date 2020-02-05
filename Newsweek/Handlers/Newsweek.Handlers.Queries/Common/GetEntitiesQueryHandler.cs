﻿namespace Newsweek.Handlers.Queries.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    
    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Contracts;
    
    public class GetEntitiesQueryHandler<TEntity> : IQueryHandler<GetEntitiesQuery<TEntity>, IEnumerable<TEntity>>
        where TEntity : BaseModel<int>
    {
        private readonly NewsweekDbContext dbContext;

        public GetEntitiesQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> Handle(GetEntitiesQuery<TEntity> query)
        {
            IQueryable<TEntity> entities = dbContext.Set<TEntity>();

            if (query.Filter != null)
            {
                entities = entities.Where(query.Filter);
            }

            if (query.Selector != null)
            {
                entities = entities.Select(query.Selector);
            }

            if (query.OrderBy != null)
            {
                entities = query.OrderBy(entities);
            }

            return await entities.ToListAsync();
        }
    }
}