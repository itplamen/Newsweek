﻿namespace Newsweek.Handlers.Commands.News
{
    using System.Collections.Generic;

    using AutoMapper;

    using MediatR;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Handlers.Commands.Tags;

    public class NewsCommand : IRequest, IMapTo<News>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string RemoteUrl { get; set; }

        public string MainImageUrl { get; set; }

        public int SourceId { get; set; }

        public int SubcategoryId { get; set; }

        public SubcategoryCommand Subcategory { get; set; }

        public IEnumerable<TagCommand> Tags { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<NewsCommand, News>()
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.RemoteUrl, opt => opt.MapFrom(x => x.RemoteUrl))
                .ForMember(x => x.MainImageUrl, opt => opt.MapFrom(x => x.MainImageUrl))
                .ForMember(x => x.SourceId, opt => opt.MapFrom(x => x.SourceId))
                .ForMember(x => x.SubcategoryId, opt => opt.MapFrom(x => x.SubcategoryId))
                .ForMember(x => x.Subcategory, opt => opt.Ignore())
                .ForMember(x => x.Tags, opt => opt.Ignore());
        }
    }
}