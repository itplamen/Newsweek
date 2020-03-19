using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Newsweek.Web.Areas.Identity.IdentityHostingStartup))]
namespace Newsweek.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}