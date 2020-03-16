namespace Newsweek.Common.Infrastructure.Logging
{
    public interface ILoggerStorage
    {
        void Save(LogData logData);

        bool HasAny();

        LogData Get();
    }
}