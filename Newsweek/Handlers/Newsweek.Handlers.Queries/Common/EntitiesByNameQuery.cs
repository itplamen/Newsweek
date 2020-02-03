namespace Newsweek.Handlers.Queries.Common
{
    using System.Collections.Generic;

    using Newsweek.Data.Models;
    using Newsweek.Data.Models.Contracts;
    using Newsweek.Handlers.Queries.Contracts;

    public class EntitiesByNameQuery<TEntity, TKey> : IQuery<IEnumerable<TEntity>>
        where TEntity : BaseModel<TKey>, INameSearchableEntity
    {
        public EntitiesByNameQuery(IEnumerable<string> names)
        {
            Names = names;
        }

        public IEnumerable<string> Names { get; set; }
    }
}