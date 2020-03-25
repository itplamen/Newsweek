namespace Newsweek.Handlers.Commands.Common.Update
{
    using MediatR;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    
    public class UpdateEntityCommand<TEntity, TSource> : IRequest<bool>
        where TEntity : BaseModel<int>
        where TSource : IMapTo<TEntity>
    {
        public UpdateEntityCommand(TSource source)
        {
            Source = source;
        }

        public TSource Source { get; set; }
    }
}