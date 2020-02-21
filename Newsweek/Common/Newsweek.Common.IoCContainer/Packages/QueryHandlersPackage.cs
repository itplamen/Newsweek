namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;
    
    using MediatR;

    using Microsoft.Extensions.DependencyInjection;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;

    public sealed class QueryHandlersPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<News>, IEnumerable<News>>, GetEntitiesQueryHandler<News>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Tag>, IEnumerable<Tag>>, GetEntitiesQueryHandler<Tag>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Source>, IEnumerable<Source>>, GetEntitiesQueryHandler<Source>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Category>, IEnumerable<Category>>, GetEntitiesQueryHandler<Category>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Subcategory>, IEnumerable<Subcategory>>, GetEntitiesQueryHandler<Subcategory>>();
            services.AddScoped<IQueryHandler<SelectEntitiesQuery<Source, int>, IEnumerable<int>>, SelectEntitiesQueryHandler<Source, int>>();
        }
    }
}