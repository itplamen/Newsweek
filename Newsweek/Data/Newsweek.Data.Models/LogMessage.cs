namespace Newsweek.Data.Models
{
    using System;

    public class LogMessage : BaseModel<int>
    {
        public string Action { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public TimeSpan Duration { get; set; }

        public LogMessageType Type { get; set; }
    }
}