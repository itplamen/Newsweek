namespace Newsweek.Web.ViewModels.News
{
    using AutoMapper;
    
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Web.ViewModels.Comments;
    using Newsweek.Web.ViewModels.Sources;

    public class NewsViewModel : NewsBaseViewModel, IHaveCustomMappings
    {
        public string Content { get; set; }

        public string RemoteUrl { get; set; }

        public SourceViewModel Source { get; set; }

        public CommentsListViewModel CommentsList { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.News, NewsViewModel>()
                .ForMember(x => x.CommentsList, opt => opt.MapFrom(x => x.Comments));
        }
    }
}