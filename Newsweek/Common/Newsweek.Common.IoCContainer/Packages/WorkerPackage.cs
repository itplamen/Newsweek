namespace Newsweek.Common.IoCContainer.Packages
{
    using System.Reflection;

    using AngleSharp.Html.Parser;
    
    using MediatR;
    
    using Microsoft.Extensions.DependencyInjection;
   
    using Newsweek.Common.Constants;
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Worker.Core.Api;
    using Newsweek.Worker.Core.Contracts;
    using Newsweek.Worker.Core.Providers;
    using Newsweek.Worker.Core.Tasks;

    public sealed class WorkerPackage : IPackage
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IHtmlParser, HtmlParser>();
            services.AddSingleton<INewsApi, NewsApi>();

            services.AddTransient<EuropeNewsProvider>();
            services.AddTransient<WorldNewsProvider>();
            services.AddTransient<SportNewsProvider>();
            services.AddTransient<ITNewsProvider>();

            RegisterNewsProviderDecorator<EuropeNewsProvider>(services);
            RegisterNewsProviderDecorator<WorldNewsProvider>(services);
            RegisterNewsProviderDecorator<SportNewsProvider>(services);
            RegisterNewsProviderDecorator<ITNewsProvider>(services);

            services.AddTransient<ITask, NewsTask>(x =>
                new NewsTask(x.GetRequiredService<IMediator>(), x.GetServices<INewsProvider>())); 

            AutoMapperConfig.RegisterMappings(Assembly.Load(PublicConstants.COMMANDS_ASSEMBLY));
            services.AddMediatR(Assembly.Load(PublicConstants.COMMANDS_ASSEMBLY), Assembly.Load(PublicConstants.QUERIES_ASSEMBLY));
        }

        private void RegisterNewsProviderDecorator<TNewsProvider>(IServiceCollection services)
            where TNewsProvider : INewsProvider
        {
            services.AddTransient<INewsProvider, NewsProviderLogging>(x =>
                new NewsProviderLogging(x.GetRequiredService<IMediator>(), x.GetRequiredService<TNewsProvider>()));
        }
    }
}