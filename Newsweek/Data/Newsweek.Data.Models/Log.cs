namespace Newsweek.Data.Models
{
    using Microsoft.Extensions.Logging;

    public class Log : BaseModel<int>
    {
        public string Operation { get; set; }

        public string Message { get; set; }

        public string Scope { get; set; }

        public LogLevel Level { get; set; }
    }
}