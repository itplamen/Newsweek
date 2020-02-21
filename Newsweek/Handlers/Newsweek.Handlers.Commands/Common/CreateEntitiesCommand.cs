namespace Newsweek.Handlers.Commands.Common
{
    using System.Collections.Generic;

    using MediatR;
    
    using Newsweek.Data.Models;

    public class CreateEntitiesCommand<TEntity, TKey> : IRequest<IEnumerable<TEntity>>
        where TEntity : BaseModel<TKey>
    {
        public CreateEntitiesCommand(IEnumerable<IRequest> entities)
        {
            Entities = entities;
        }

        public IEnumerable<IRequest> Entities { get; set; }
    }
}