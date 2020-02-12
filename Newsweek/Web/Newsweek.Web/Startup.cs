namespace Newsweek.Web
{
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
 
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Common.IoCContainer;
    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Data.Seeders;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Handlers.Queries.News;
    using Newsweek.Web.Models.Common;
    using Newsweek.Web.Models.News;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NewsweekDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(IdentityOptionsProvider.GetIdentityOptions)
                .AddEntityFrameworkStores<NewsweekDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton(configuration);

            services.AddServices();

            services.AddScoped<IQueryHandler<IEnumerable<NewsViewModel>>, TopNewsQueryHandler<NewsViewModel>>();
            services.AddScoped<IQueryHandler<SelectEntitiesQuery<News, NewsViewModel>, IEnumerable<NewsViewModel>>, SelectEntitiesQueryHandler<News, NewsViewModel>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<NewsweekDbContext>();
                dbContext.Database.Migrate();

                NewsweekDbContextSeeder.Seed(dbContext);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    "news",
                    "News/{id:int:min(1)}",
                    new { controller = "News", action = "Get", });
                endpoints.MapRazorPages();
            });

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }
    }
}