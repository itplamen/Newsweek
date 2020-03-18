namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;
    
    using MediatR;
    
    using Microsoft.Extensions.DependencyInjection;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Emails;

    public sealed class CommandHandlersPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<SendEmailCommand>, SendEmailCommandHandler>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Tag>, IEnumerable<Tag>>, CreateEntitiesCommandHandler<Tag>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<News>, IEnumerable<News>>, CreateEntitiesCommandHandler<News>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<NewsTag>, IEnumerable<NewsTag>>, CreateEntitiesCommandHandler<NewsTag>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Subcategory>, IEnumerable<Subcategory>>, CreateEntitiesCommandHandler<Subcategory>>();
        }
    }
}