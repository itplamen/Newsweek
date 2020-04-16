namespace Newsweek.Handlers.Queries.Dashboard
{
    using System.Collections.Generic;

    public class DashboardResult
    {
        public NewsCountResult NewsCount { get; set; }

        public IEnumerable<NewsByMonthCountResult> NewsByMonthCount { get; set; }
    }
}