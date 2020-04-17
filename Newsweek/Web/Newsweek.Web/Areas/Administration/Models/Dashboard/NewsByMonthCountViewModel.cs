namespace Newsweek.Web.Areas.Administration.Models.Dashboard
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Handlers.Queries.Dashboard;

    public class NewsByMonthCountViewModel : IMapFrom<NewsByMonthCountResult>
    {
        public int Month { get; set; }

        public int NewsCount { get; set; }
    }
}