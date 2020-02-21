namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;
    
    using MediatR;
    
    using Microsoft.Extensions.DependencyInjection;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Commands.Subcategories;
    using Newsweek.Handlers.Commands.Tags;

    public sealed class CommandHandlersPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<ICommandHandler<CreateNewsCommand>, CreateNewsCommandHandler>();
            services.AddScoped<ICommandHandler<CreateTagsCommand>, CreateTagsCommandHandler>();
            services.AddScoped<ICommandHandler<CreateSubcategoriesCommand, IEnumerable<Subcategory>>, CreateSubcategoriesCommandHandler>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Tag, int>, IEnumerable<Tag>>, CreateEntitiesCommandHandler<Tag, int>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<News, int>, IEnumerable<News>>, CreateEntitiesCommandHandler<News, int>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Subcategory, int>, IEnumerable<Subcategory>>, CreateEntitiesCommandHandler<Subcategory, int>>();
        }
    }
}