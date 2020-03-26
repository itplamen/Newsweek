namespace Newsweek.Handlers.Queries.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using MediatR;
    
    using Newsweek.Data.Models;
    
    public class SelectEntitiesQuery<TEntity, TResult> : IRequest<IEnumerable<TResult>>
        where TEntity : BaseModel<int>
    {
        public int Take { get; set; }

        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        public Expression<Func<TEntity, TResult>> Selector { get; set; }
    }
}