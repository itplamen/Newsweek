namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;
    
    using MediatR;

    using Microsoft.Extensions.DependencyInjection;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;

    public sealed class QueryHandlersPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetEntitiesQuery<News>, IEnumerable<News>>, GetEntitiesQueryHandler<News>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Tag>, IEnumerable<Tag>>, GetEntitiesQueryHandler<Tag>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<NewsTag>, IEnumerable<NewsTag>>, GetEntitiesQueryHandler<NewsTag>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Source>, IEnumerable<Source>>, GetEntitiesQueryHandler<Source>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Category>, IEnumerable<Category>>, GetEntitiesQueryHandler<Category>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Subcategory>, IEnumerable<Subcategory>>, GetEntitiesQueryHandler<Subcategory>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Source, int>, IEnumerable<int>>, GetEntitiesQueryHandler<Source, int>>();
        }
    }
}