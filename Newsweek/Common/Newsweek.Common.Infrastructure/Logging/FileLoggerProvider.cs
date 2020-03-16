namespace Newsweek.Common.Infrastructure.Logging
{
    using System;
    using System.IO;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly IConfiguration configuration;

        public FileLoggerProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger("logs.txt", configuration["Logging:Directory"], categoryName);
        }

        public void Dispose()
        {
        }

        public class Logger : ILogger
        {
            private readonly string file;
            private readonly string directory;
            private readonly string categoryName;

            private string scope;

            public Logger(string file, string directory, string categoryName)
            {
                this.file = file;
                this.directory = directory;
                this.categoryName = categoryName;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                scope = JsonSerializer.Serialize(state);

                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logLevel >= LogLevel.Information && 
                    !string.IsNullOrWhiteSpace(categoryName) &&
                    !string.IsNullOrWhiteSpace(scope);
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (IsEnabled(logLevel))
                {
                    string fullDirectoryPath = CreateDirectory(directory);
                    string msg = formatter(state, exception);

                    string text = JsonSerializer.Serialize(new
                    {
                        logLevel,
                        categoryName,
                        eventId,
                        msg
                    });

                    File.AppendAllText($@"{fullDirectoryPath}\{file}", text);
                }
            }

            private string CreateDirectory(string directory)
            {
                string fullDirectoryPath = $@"{directory}\{categoryName}";

                if (!Directory.Exists(fullDirectoryPath))
                {
                    Directory.CreateDirectory(fullDirectoryPath);
                }

                return fullDirectoryPath;
            }
        }
    }
}