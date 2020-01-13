namespace Newsweek.Handlers.Commands.Contracts
{
    using System.Threading.Tasks;

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}