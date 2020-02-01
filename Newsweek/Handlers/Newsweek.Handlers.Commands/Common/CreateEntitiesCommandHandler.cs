namespace Newsweek.Handlers.Commands.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    using AutoMapper;
    
    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;

    public class CreateEntitiesCommandHandler<TEntity, TKey> : ICommandHandler<CreateEntitiesCommand<TEntity, TKey>, Task<IEnumerable<TEntity>>>
        where TEntity : BaseModel<TKey>
    {
        private readonly NewsweekDbContext dbContext;

        public CreateEntitiesCommandHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> Handle(CreateEntitiesCommand<TEntity, TKey> command)
        {
            if (command.Entities.Any())
            {
                IEnumerable<TEntity> entities = Mapper.Map<IEnumerable<TEntity>>(command.Entities);

                foreach (var entity in entities)
                {
                    entity.CreatedOn = DateTime.UtcNow;

                    await dbContext.AddAsync(entity);
                }

                await dbContext.SaveChangesAsync();

                return entities;
            }

            return Enumerable.Empty<TEntity>();
        }
    }
}