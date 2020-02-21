namespace Newsweek.Handlers.Queries.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    
    using MediatR;
    
    using Newsweek.Data.Models;
    
    public class GetEntitiesQuery<TEntity> : IRequest<IEnumerable<TEntity>>
        where TEntity : BaseModel<int>
    {
        public int Take { get; set; }

        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; set; }
    }
}