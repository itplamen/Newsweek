namespace Newsweek.Handlers.Queries.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using MediatR;

    using Newsweek.Data.Models.Contracts;

    public class GetEntitiesQuery<TEntity> : GetEntitiesBaseQuery<TEntity>, IRequest<IEnumerable<TEntity>>
        where TEntity : IAuditInfo, IDeletableEntity
    {
    }

    public class GetEntitiesQuery<TEntity, TResult> : GetEntitiesBaseQuery<TEntity>, IRequest<IEnumerable<TResult>>
        where TEntity : IAuditInfo, IDeletableEntity
    {
        public Expression<Func<TEntity, TResult>> Selector { get; set; }
    }
}