namespace Newsweek.Handlers.Queries.Dashboard
{
    public class NewsCountResult
    {
        public int Total { get; set; }

        public int Deleted { get; set; }

        public int Active { get; set; }

        public int Today { get; set; }

        public int Yesterday { get; set; }
    }
}