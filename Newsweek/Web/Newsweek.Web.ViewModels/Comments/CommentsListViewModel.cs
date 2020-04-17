namespace Newsweek.Web.ViewModels.Comments
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class CommentsListViewModel : IMapFrom<IEnumerable<Comment>>, IHaveCustomMappings
    {
        private const int TAKE_COMMENTS = 3;

        public int TotalCount { get; set; }

        public IEnumerable<GetCommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<IEnumerable<Comment>, CommentsListViewModel>()
                .ForMember(x => x.TotalCount, opt => opt.MapFrom(x => x.Where(y => !y.IsDeleted).Count()))
                .ForMember(x => x.Comments, opt => opt.MapFrom(x => x.Where(y => !y.IsDeleted).Take(TAKE_COMMENTS)));
        }
    }
}