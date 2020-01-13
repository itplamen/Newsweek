namespace Newsweek.Handlers.Commands.Common
{
    using System.Collections.Generic;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;

    public class CreateEntitiesCommand<TEntity, TKey> : ICommand
        where TEntity : BaseModel<TKey>
    {
        public CreateEntitiesCommand(IEnumerable<ICommand> entities)
        {
            Entities = entities;
        }

        public IEnumerable<ICommand> Entities { get; set; }
    }
}