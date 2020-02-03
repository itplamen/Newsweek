namespace Newsweek.Web.Models.Sources
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class SourceViewModel : IMapFrom<Source>
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }
}