namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;

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
            services.AddScoped<IQueryHandler<GetEntitiesQuery<News>, IEnumerable<News>>, GetEntitiesQueryHandler<News>>();
            services.AddScoped<IQueryHandler<GetEntitiesQuery<Source>, IEnumerable<Source>>, GetEntitiesQueryHandler<Source>>();
            services.AddScoped<IQueryHandler<GetEntitiesQuery<Category>, IEnumerable<Category>>, GetEntitiesQueryHandler<Category>>();
            services.AddScoped<IQueryHandler<GetEntitiesQuery<Subcategory>, IEnumerable<Subcategory>>, GetEntitiesQueryHandler<Subcategory>>();
            services.AddScoped<IQueryHandler<SelectEntitiesQuery<Source, int>, IEnumerable<int>>, SelectEntitiesQueryHandler<Source, int>>();
        }
    }
}