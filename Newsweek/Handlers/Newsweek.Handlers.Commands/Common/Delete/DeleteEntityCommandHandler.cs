namespace Newsweek.Handlers.Commands.Common.Delete
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Newsweek.Data;
    using Newsweek.Data.Models;

    public class DeleteEntityCommandHandler<TEntity> : IRequestHandler<DeleteEntityCommand<TEntity>, bool>
        where TEntity : BaseModel<int>
    {
        private readonly NewsweekDbContext dbContext;

        public DeleteEntityCommandHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteEntityCommand<TEntity> request, CancellationToken cancellationToken)
        {
            TEntity entity = dbContext.Set<TEntity>()
                .FirstOrDefault(x => x.Id == request.Id);

            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.DeletedOn = DateTime.UtcNow;

                await dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }

            return false;
        }
    }
}