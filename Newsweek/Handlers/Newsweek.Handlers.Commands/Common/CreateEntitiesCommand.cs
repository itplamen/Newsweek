namespace Newsweek.Handlers.Commands.Common
{
    using System.Collections.Generic;

    using MediatR;
    
    using Newsweek.Data.Models.Contracts;

    public class CreateEntitiesCommand<TEntity> : IRequest<IEnumerable<TEntity>>
        where TEntity : IAuditInfo, IDeletableEntity
    {
        public CreateEntitiesCommand(IEnumerable<IRequest> entities)
        {
            Entities = entities;
        }

        public IEnumerable<IRequest> Entities { get; set; }
    }
}