namespace Newsweek.Handlers.Commands.Common.Delete
{
    using MediatR;
    
    using Newsweek.Data.Models;

    public class DeleteEntityCommand<TEntity> : IRequest<bool>
        where TEntity : BaseModel<int>
    {
        public DeleteEntityCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}