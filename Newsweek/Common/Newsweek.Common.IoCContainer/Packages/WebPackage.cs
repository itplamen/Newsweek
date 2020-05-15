namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;
    using System.Reflection;
    
    using MediatR;

    using Microsoft.Extensions.DependencyInjection;

    using Newsweek.Common.Constants;
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common.Update;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Dashboard;
    using Newsweek.Handlers.Queries.News.Search;
    using Newsweek.Handlers.Queries.News.Top;
    using Newsweek.Web.ViewModels.Comments;
    using Newsweek.Web.ViewModels.Common;
    using Newsweek.Web.ViewModels.Menu;
    using Newsweek.Web.ViewModels.News;
    using Newsweek.Web.ViewModels.Sources;

    public sealed class WebPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<DashboardQuery, DashboardResult>, DashboardQueryHandler>();
            services.AddScoped<IRequestHandler<TopNewsQuery<NewsBaseViewModel>, IEnumerable<NewsBaseViewModel>>, TopNewsQueryHandler<NewsBaseViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<News, NewsBaseViewModel>, IEnumerable<NewsBaseViewModel>>, GetEntitiesQueryHandler<News, NewsBaseViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<News, NewsViewModel>, IEnumerable<NewsViewModel>>, GetEntitiesQueryHandler<News, NewsViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Comment, GetCommentViewModel>, IEnumerable<GetCommentViewModel>>, GetEntitiesQueryHandler<Comment, GetCommentViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Category, MenuViewModel>, IEnumerable<MenuViewModel>>, GetEntitiesQueryHandler<Category, MenuViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Source, SourceFullViewModel>, IEnumerable<SourceFullViewModel>>, GetEntitiesQueryHandler<Source, SourceFullViewModel>>();
            services.AddScoped<IRequestHandler<UpdateEntityCommand<Comment, UpdateCommentViewModel>, bool>, UpdateEntityCommandHandler<Comment, UpdateCommentViewModel>>();
            services.AddScoped<IRequestHandler<SearchNewsQuery, SearchNewsResponse>, SearchNewsQueryHandler>();

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, Assembly.Load(PublicConstants.COMMANDS_ASSEMBLY));
            services.AddMediatR(Assembly.Load(PublicConstants.QUERIES_ASSEMBLY), Assembly.Load(PublicConstants.COMMANDS_ASSEMBLY));
        }
    }
}