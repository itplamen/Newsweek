using AngleSharp.Html.Parser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newsweek.Worker.Core.Api;
using Newsweek.Worker.Core.Contracts;
using Newsweek.Worker.Core.Providers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Newsweek.Worker.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHostBuilder hostBuilder = new HostBuilder()
                .ConfigureServices(ConfigureServices);

            await hostBuilder.RunConsoleAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IHtmlParser, HtmlParser>();

            services.AddSingleton<INewsApi, NewsApi>();
            services.AddTransient<INewsProvider, EuronewsProvider>();

            services.AddHostedService<TasksExecutor>();
        }
    }
}
