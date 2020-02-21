namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;
    
    using MediatR;
    
    using Microsoft.Extensions.DependencyInjection;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;

    public sealed class CommandHandlersPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Tag, int>, IEnumerable<Tag>>, CreateEntitiesCommandHandler<Tag, int>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<News, int>, IEnumerable<News>>, CreateEntitiesCommandHandler<News, int>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Subcategory, int>, IEnumerable<Subcategory>>, CreateEntitiesCommandHandler<Subcategory, int>>();
        }
    }
}