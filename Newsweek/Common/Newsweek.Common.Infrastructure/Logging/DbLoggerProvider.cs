namespace Newsweek.Common.Infrastructure.Logging
{
    using System;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly ILoggerStorage loggerStorage;

        public DbLoggerProvider(ILoggerStorage loggerStorage)
        {
            this.loggerStorage = loggerStorage;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger(categoryName, loggerStorage);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public class Logger : ILogger
        {
            private readonly ILoggerStorage loggerStorage;
            private readonly string categoryName;
            private string scope;

            public Logger(string categoryName, ILoggerStorage loggerStorage)
            {
                this.categoryName = categoryName;
                this.loggerStorage = loggerStorage;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                scope = JsonSerializer.Serialize(state);

                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= LogLevel.Information && !string.IsNullOrEmpty(categoryName);
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (IsEnabled(logLevel))
                {
                    string msg = formatter(state, exception);

                    LogData logData = new LogData()
                    {
                        Scope = scope,
                        Level = logLevel,
                        Operation = categoryName,
                        Message = JsonSerializer.Serialize(new
                        {
                            msg,
                            eventId
                        })
                    };

                    loggerStorage.Save(logData);
                }
            }
        }
    }
}