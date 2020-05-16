namespace Newsweek.Handlers.Commands.LogMessages
{
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;
    
    using Microsoft.Extensions.Logging;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common.Create;

    public class LogMessageCommandHandler : IRequestHandler<LogMessageCommand>
    {
        private readonly IMediator mediator;
        private readonly ILogger<LogMessageCommandHandler> logger;

        public LogMessageCommandHandler(IMediator mediator, ILogger<LogMessageCommandHandler> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task<Unit> Handle(LogMessageCommand request, CancellationToken cancellationToken)
        {
            foreach (var logMessage in request.LogMessages)
            {
                if (logMessage.LogToFile)
                {
                    logger.LogInformation(
                        $"{logMessage.Action}, HasErrors - {logMessage.HasErrors}",
                        logMessage.Request,
                        logMessage.Response,
                        logMessage.Action);
                }
            }

            await mediator.Send(new CreateEntitiesCommand<LogMessage>(request.LogMessages), cancellationToken);

            return Unit.Value;
        }
    }
}