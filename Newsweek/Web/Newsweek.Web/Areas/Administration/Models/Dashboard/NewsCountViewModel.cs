namespace Newsweek.Web.Areas.Administration.Models.Dashboard
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Handlers.Queries.Dashboard;

    public class NewsCountViewModel : IMapFrom<NewsCountResult>
    {
        public int Total { get; set; }

        public int Deleted { get; set; }

        public int Active { get; set; }

        public int Today { get; set; }

        public int Yesterday { get; set; }
    }
}