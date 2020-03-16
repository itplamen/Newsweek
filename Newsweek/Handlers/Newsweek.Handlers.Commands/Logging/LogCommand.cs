namespace Newsweek.Handlers.Commands.Logging
{
    using MediatR;

    using Microsoft.Extensions.Logging;

    using Newsweek.Common.Infrastructure.Logging;
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class LogCommand : IRequest, IMapFrom<LogData>, IMapTo<Log>
    {
        public string Operation { get; set; }

        public string Message { get; set; }

        public string Scope { get; set; }

        public LogLevel Level { get; set; }
    }
}