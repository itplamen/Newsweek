namespace Newsweek.Data.Models
{
    public class Log : BaseModel<int>
    {
        public string Operation { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public string Message { get; set; }
    }
}