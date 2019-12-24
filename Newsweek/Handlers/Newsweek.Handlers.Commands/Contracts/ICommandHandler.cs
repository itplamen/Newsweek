namespace Newsweek.Handlers.Commands.Contracts
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}