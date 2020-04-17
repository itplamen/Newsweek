namespace Newsweek.Web.ViewModels.Sources
{
    using AutoMapper;
    
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class SourceFullViewModel : SourceViewModel, IHaveCustomMappings
    {
        public string Description { get; set; }

        public int NewsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Source, SourceFullViewModel>()
                .ForMember(x => x.NewsCount, opt => opt.MapFrom(x => x.News.Count));
        }
    }
}