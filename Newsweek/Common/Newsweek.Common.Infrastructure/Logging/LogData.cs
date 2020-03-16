namespace Newsweek.Common.Infrastructure.Logging
{
    using Microsoft.Extensions.Logging;

    public class LogData
    {
        public string Operation { get; set; }

        public string Message { get; set; }

        public string Scope { get; set; }

        public LogLevel Level { get; set; }
    }
}