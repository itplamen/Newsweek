namespace Newsweek.Handlers.Commands.Logging
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Newsweek.Common.Infrastructure.Logging;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;

    public class LogCommandHandler : IRequestHandler<LogCommand>
    {
        private readonly IMediator mediator;
        private readonly ILoggerStorage loggerStorage;

        public LogCommandHandler(IMediator mediator, ILoggerStorage loggerStorage)
        {
            this.mediator = mediator;
            this.loggerStorage = loggerStorage;
        }

        public async Task<Unit> Handle(LogCommand request, CancellationToken cancellationToken)
        {
            ICollection<LogCommand> logCommands = new List<LogCommand>();

            while (loggerStorage.HasAny())
            {
                LogData logData = loggerStorage.Get();
                LogCommand logCommand =  Mapper.Map<LogCommand>(logData);

                logCommands.Add(logCommand);
            }

            await mediator.Send(new CreateEntitiesCommand<Log>(logCommands), cancellationToken);

            return Unit.Value;
        }
    }
}