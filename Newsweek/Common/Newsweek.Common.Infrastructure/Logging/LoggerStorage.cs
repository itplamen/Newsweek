namespace Newsweek.Common.Infrastructure.Logging
{
    using System.Collections.Concurrent;

    public class LoggerStorage : ILoggerStorage
    {
        private readonly ConcurrentQueue<LogData> storage;

        public LoggerStorage()
        {
            storage = new ConcurrentQueue<LogData>();
        }

        public void Save(LogData logData)
        {
            storage.Enqueue(logData);
        }

        public bool HasAny()
        {
            return storage.TryPeek(out LogData logData);
        }

        public LogData Get()
        {
            storage.TryDequeue(out LogData logData);

            return logData;
        }
    }
}