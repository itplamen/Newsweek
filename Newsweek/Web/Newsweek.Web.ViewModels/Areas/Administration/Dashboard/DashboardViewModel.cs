﻿namespace Newsweek.Web.ViewModels.Areas.Administration.Dashboard
{
    using System.Collections.Generic;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Handlers.Queries.Dashboard;

    public class DashboardViewModel : IMapFrom<DashboardResult>
    {
        public NewsCountResult NewsCount { get; set; }

        public IEnumerable<NewsByMonthCountResult> NewsByMonthCount { get; set; }
    }
}