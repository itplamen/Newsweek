namespace Newsweek.Handlers.Commands.Contracts
{
    using System.Threading.Tasks;

    public interface ICommandDispatcher
    {
        Task Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand;

        Task<TResult> Dispatch<TCommand, TResult>(TCommand command)
            where TCommand : ICommand;
    }
}