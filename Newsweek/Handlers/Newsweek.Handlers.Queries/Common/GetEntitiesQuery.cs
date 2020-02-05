﻿namespace Newsweek.Handlers.Queries.Common
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
        public Expression<Func<TEntity, bool>> Filter { get; set; }

        public Expression<Func<TEntity, TEntity>> Selector { get; set; }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; set; }
    }
}