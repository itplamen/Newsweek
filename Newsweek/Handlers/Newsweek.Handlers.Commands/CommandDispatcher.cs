namespace Newsweek.Handlers.Commands
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    using Newsweek.Handlers.Commands.Contracts;

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            ICommandHandler<TCommand> handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

            await handler.Handle(command);
        }

        public async Task<TResult> Dispatch<TCommand, TResult>(TCommand command)
            where TCommand : ICommand
        {
            ICommandHandler<TCommand, TResult> handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();

            return await handler.Handle(command);
        }
    }
}