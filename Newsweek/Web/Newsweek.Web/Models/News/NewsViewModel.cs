﻿namespace Newsweek.Web.Models.News
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Web.Models.Comments;
    using Newsweek.Web.Models.Common;
    using Newsweek.Web.Models.Sources;
    using Newsweek.Web.Models.Subcategories;

    public class NewsViewModel : BaseViewModel, IMapFrom<Data.Models.News>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string RemoteUrl { get; set; }

        public string MainImageUrl { get; set; }

        public SourceViewModel Source { get; set; }

        public SubcategoryViewModel Subcategory { get; set; }

        public IEnumerable<GetCommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.News, NewsViewModel>()
                .ForMember(x => x.Comments, opt => opt.MapFrom(x => x.Comments.Where(y => !y.IsDeleted)));
        }
    }
}