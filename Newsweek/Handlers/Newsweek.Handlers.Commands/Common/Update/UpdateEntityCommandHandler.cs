namespace Newsweek.Handlers.Commands.Common.Update
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;
    
    using MediatR;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data;
    using Newsweek.Data.Models;
    
    public class UpdateEntityCommandHandler<TEntity, TSource> : IRequestHandler<UpdateEntityCommand<TEntity, TSource>, bool>
        where TEntity : BaseModel<int>
        where TSource : IMapTo<TEntity>
    {
        private readonly NewsweekDbContext dbContext;

        public UpdateEntityCommandHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Handle(UpdateEntityCommand<TEntity, TSource> request, CancellationToken cancellationToken)
        {
            TEntity entity = Mapper.Map<TEntity>(request.Source);
            TEntity entityToUpdate = dbContext.Set<TEntity>().FirstOrDefault(x => x.Id == entity.Id);

            if (entityToUpdate != null)
            {
                var propertiesToUpdate = request.Source.GetType().GetProperties()
                    .Where(x => x.Name != nameof(entity.Id));

                foreach (var property in propertiesToUpdate)
                {
                    var valueToUpdate = property.GetValue(request.Source);
                    entityToUpdate.GetType().GetProperty(property.Name).SetValue(entityToUpdate, valueToUpdate);
                }

                entityToUpdate.ModifiedOn = DateTime.UtcNow;

                await dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }

            return false;
        }
    }
}