namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Collections.Generic;
    
    using MediatR;
    
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Newsweek.Common.Infrastructure.Logging;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common.Create;
    using Newsweek.Handlers.Commands.Common.Delete;
    using Newsweek.Handlers.Commands.Emails;
    using Newsweek.Handlers.Commands.LogMessages;

    public sealed class CommandHandlersPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<SendEmailCommand>, SendEmailCommandHandler>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Tag>, IEnumerable<Tag>>, CreateEntitiesCommandHandler<Tag>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<News>, IEnumerable<News>>, CreateEntitiesCommandHandler<News>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<NewsTag>, IEnumerable<NewsTag>>, CreateEntitiesCommandHandler<NewsTag>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Comment>, IEnumerable<Comment>>, CreateEntitiesCommandHandler<Comment>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<Subcategory>, IEnumerable<Subcategory>>, CreateEntitiesCommandHandler<Subcategory>>();
            services.AddScoped<IRequestHandler<CreateEntitiesCommand<LogMessage>, IEnumerable<LogMessage>>, CreateEntitiesCommandHandler<LogMessage>>();
            services.AddScoped<IRequestHandler<DeleteEntityCommand<Comment>, bool>, DeleteEntityCommandHandler<Comment>>();
            services.AddScoped<IRequestHandler<DeleteEntityCommand<News>, bool>, DeleteEntityCommandHandler<News>>();
            services.AddScoped<IRequestHandler<LogMessageCommand>, LogMessageCommandHandler>();
            services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
        }
    }
}