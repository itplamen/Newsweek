namespace Newsweek.Worker.Console
{
    using System.IO;
    using System.Threading.Tasks;
    
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Newsweek.Common.IoCContainer;
    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Data.Seeders;
    using Newsweek.Worker.Core.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHostBuilder hostBuilder = new HostBuilder()
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });

            await hostBuilder.RunConsoleAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton(configuration);

            services.AddDbContext<NewsweekDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<NewsweekDbContext>();

            using (var serviceScope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<NewsweekDbContext>();
                dbContext.Database.Migrate();

                NewsweekDbContextSeeder.Seed(dbContext);
            }

            services.AddWorkerServices();
            services.AddHostedService<TasksExecutor>();
        }
    }
}