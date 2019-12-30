namespace Newsweek.Handlers.Queries.Common
{
    using Newsweek.Data.Models;
    using Newsweek.Data.Models.Contracts;
    using Newsweek.Handlers.Queries.Contracts;

    public class EntityByNameQuery<TEntity, TKey> : IQuery<TEntity>
        where TEntity : BaseModel<TKey>, INameSearchableEntity
    {
        public EntityByNameQuery(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}