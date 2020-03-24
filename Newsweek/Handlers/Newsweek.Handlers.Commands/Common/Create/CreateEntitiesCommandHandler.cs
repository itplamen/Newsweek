namespace Newsweek.Handlers.Commands.Common.Create
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using AutoMapper;

    using MediatR;

    using Newsweek.Data;
    using Newsweek.Data.Models.Contracts;

    public class CreateEntitiesCommandHandler<TEntity> : IRequestHandler<CreateEntitiesCommand<TEntity>, IEnumerable<TEntity>>
        where TEntity : IAuditInfo, IDeletableEntity
    {
        private readonly NewsweekDbContext dbContext;

        public CreateEntitiesCommandHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> Handle(CreateEntitiesCommand<TEntity> request, CancellationToken cancellationToken)
        {
            if (request.Entities.Any())
            {
                IEnumerable<TEntity> entities = Mapper.Map<IEnumerable<TEntity>>(request.Entities);

                foreach (var entity in entities)
                {
                    entity.CreatedOn = DateTime.UtcNow;

                    await dbContext.AddAsync(entity);
                }

                await dbContext.SaveChangesAsync(cancellationToken);

                return entities;
            }

            return Enumerable.Empty<TEntity>();
        }
    }
}