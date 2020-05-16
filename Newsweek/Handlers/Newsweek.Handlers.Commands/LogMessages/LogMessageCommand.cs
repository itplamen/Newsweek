namespace Newsweek.Handlers.Commands.LogMessages
{
    using System.Collections.Generic;

    using MediatR;

    public class LogMessageCommand : IRequest
    {
        public LogMessageCommand(IEnumerable<MessageCommand> logMessages)
        {
            LogMessages = logMessages;
        }

        public IEnumerable<MessageCommand> LogMessages { get; set; }
    }
}