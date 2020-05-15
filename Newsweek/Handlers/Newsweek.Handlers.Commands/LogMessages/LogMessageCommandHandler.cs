namespace Newsweek.Handlers.Commands.LogMessages
{
    using System.Collections.Generic;
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
            if (request.LogToFile)
            {
                logger.LogInformation(
                    $"{request.Action}, HasErrors - {request.HasErrors}", 
                    request.Request,
                    request.Response,
                    request.Action);
            }

            var logEntities = new List<LogMessageCommand>() { request };
            await mediator.Send(new CreateEntitiesCommand<LogMessage>(logEntities), cancellationToken);

            return Unit.Value;
        }
    }
}