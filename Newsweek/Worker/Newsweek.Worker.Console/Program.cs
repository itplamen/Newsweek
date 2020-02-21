namespace Newsweek.Worker.Console
{
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using AngleSharp.Html.Parser;

    using MediatR;
    
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Common.IoCContainer;
    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Data.Seeders;
    using Newsweek.Worker.Core.Api;
    using Newsweek.Worker.Core.Contracts;
    using Newsweek.Worker.Core.Providers;
    using Newsweek.Worker.Core.Tasks;

    public class Program
    {
        private const string MAPPING_ASSEMBLY = "Newsweek.Handlers.Commands";

        public static async Task Main(string[] args)
        {
            IHostBuilder hostBuilder = new HostBuilder()
                .ConfigureServices(ConfigureServices);

            await hostBuilder.RunConsoleAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true).AddEnvironmentVariables().Build();

            services.AddSingleton(configuration);

            services.AddDbContext<NewsweekDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(IdentityOptionsProvider.GetIdentityOptions)
                .AddEntityFrameworkStores<NewsweekDbContext>();

            using (var serviceScope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<NewsweekDbContext>();
                dbContext.Database.Migrate();

                NewsweekDbContextSeeder.Seed(dbContext);
            }

            AutoMapperConfig.RegisterMappings(Assembly.Load(MAPPING_ASSEMBLY));
            services.AddMediatR(Assembly.Load(MAPPING_ASSEMBLY));

            services.AddHttpClient();
            services.AddSingleton<IHtmlParser, HtmlParser>();
            services.AddSingleton<INewsApi, NewsApi>();
            
            services.AddTransient<EuropeNewsProvider>();
            services.AddTransient<WorldNewsProvider>();
            services.AddTransient<SportNewsProvider>();
            services.AddTransient<ITNewsProvider>();
            services.AddTransient<ITask, NewsTask>(x =>
                new NewsTask(
                    x.GetRequiredService<IMediator>(),
                    new INewsProvider[]
                    {
                        x.GetRequiredService<EuropeNewsProvider>(),
                        x.GetRequiredService<WorldNewsProvider>(),
                        x.GetRequiredService<SportNewsProvider>(),
                        x.GetRequiredService<ITNewsProvider>()
                    }));

            services.AddServices();
            services.AddHostedService<TasksExecutor>();
        }
    }
}