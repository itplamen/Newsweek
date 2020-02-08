namespace Newsweek.Handlers.Queries.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Contracts;
    
    public class SelectEntitiesQuery<TEntity, TResult> : IQuery<IEnumerable<TResult>>
        where TEntity : BaseModel<int>
    {
        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        public Expression<Func<TEntity, TResult>> Selector { get; set; }
    }
}