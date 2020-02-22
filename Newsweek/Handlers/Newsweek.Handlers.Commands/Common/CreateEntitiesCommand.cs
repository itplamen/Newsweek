namespace Newsweek.Handlers.Commands.Common
{
    using System.Collections.Generic;

    using MediatR;
    
    using Newsweek.Data.Models;

    public class CreateEntitiesCommand<TEntity> : IRequest<IEnumerable<TEntity>>
        where TEntity : BaseModel<int>
    {
        public CreateEntitiesCommand(IEnumerable<IRequest> entities)
        {
            Entities = entities;
        }

        public IEnumerable<IRequest> Entities { get; set; }
    }
}