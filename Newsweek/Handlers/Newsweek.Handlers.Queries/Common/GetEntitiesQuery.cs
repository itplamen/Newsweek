namespace Newsweek.Handlers.Queries.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Contracts;

    public class GetEntitiesQuery<TEntity> : IQuery<IEnumerable<TEntity>>
        where TEntity : BaseModel<int>
    {
        public GetEntitiesQuery(Expression<Func<TEntity, bool>> filter)
        {
            Filter = filter;
        }

        public GetEntitiesQuery(
            Expression<Func<TEntity, bool>> filter, 
            Expression<Func<TEntity, TEntity>> selector, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            Filter = filter;
            Selector = selector;
            OrderBy = orderBy;
        }

        public GetEntitiesQuery()
        {
        }

        public Expression<Func<TEntity, bool>> Filter { get; set; }

        public Expression<Func<TEntity, TEntity>> Selector { get; set; }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; set; }
    }
}