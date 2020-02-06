namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;
    
    using Microsoft.Extensions.DependencyInjection;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.Subcategories;

    public sealed class CommandHandlersPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<ICommandHandler<CreateNewsCommand>, CreateNewsCommandHandler>();
            services.AddScoped<ICommandHandler<CreateSubcategoriesCommand, IEnumerable<Subcategory>>, CreateSubcategoriesCommandHandler>();
            services.AddScoped<ICommandHandler<CreateEntitiesCommand<News, int>, IEnumerable<News>>, CreateEntitiesCommandHandler<News, int>>();
            services.AddScoped<ICommandHandler<CreateEntitiesCommand<Subcategory, int>, IEnumerable<Subcategory>>, CreateEntitiesCommandHandler<Subcategory, int>>();
        }
    }
}