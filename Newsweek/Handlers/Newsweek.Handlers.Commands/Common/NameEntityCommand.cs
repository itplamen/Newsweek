namespace Newsweek.Handlers.Commands.Common
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;

    public class NameEntityCommand<TEntity, TKey> : ICommand, IMapTo<TEntity>
        where TEntity : BaseModel<TKey>
    {
        public NameEntityCommand(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}