namespace Newsweek.Web.ViewModels.Comments
{
    using AutoMapper;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Web.ViewModels.Common;

    public class GetCommentViewModel : BaseViewModel, IMapFrom<Comment>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public string Username { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Comment, GetCommentViewModel>()
                .ForMember(x => x.Username, opt => opt.MapFrom(x => x.User.UserName));
        }
    }
}