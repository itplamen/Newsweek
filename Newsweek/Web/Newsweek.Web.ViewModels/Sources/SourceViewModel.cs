﻿namespace Newsweek.Web.ViewModels.Sources
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Web.ViewModels.Common;

    public class SourceViewModel : BaseViewModel, IMapFrom<Source>
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }
}