namespace Newsweek.Web
{
    using System.Collections.Generic;
    using System.Reflection;

    using MediatR;
    
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using SendGrid;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Common.IoCContainer;
    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Data.Seeders;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.News;
    using Newsweek.Web.Models.Common;
    using Newsweek.Web.Models.Menu;
    using Newsweek.Web.Models.News;
    using Newsweek.Common.Infrastructure.Logging;
    using Newsweek.Handlers.Commands.Common.Update;
    using Newsweek.Web.Models.Comments;
    using Newsweek.Web.Models.Sources;

    public class Startup
    {
        private const string QUERIES_ASSEMBLY = "Newsweek.Handlers.Queries";
        private const string COMMANDS_ASSEMBLY = "Newsweek.Handlers.Commands";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NewsweekDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddEntityFrameworkStores<NewsweekDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddResponseCaching();

            services.AddSingleton(configuration);

            services.AddServices();
            services.AddMediatR(Assembly.Load(QUERIES_ASSEMBLY), Assembly.Load(COMMANDS_ASSEMBLY));

            services.AddScoped<ISendGridClient>(x => new SendGridClient(configuration["SendGrid:ApiKey"]));
            services.AddScoped<IRequestHandler<TopNewsQuery<NewsViewModel>, IEnumerable<NewsViewModel>>, TopNewsQueryHandler<NewsViewModel>>();
            services.AddScoped<IRequestHandler<SelectEntitiesQuery<News, NewsViewModel>, IEnumerable<NewsViewModel>>, SelectEntitiesQueryHandler<News, NewsViewModel>>();
            services.AddScoped<IRequestHandler<SelectEntitiesQuery<Comment, GetCommentViewModel>, IEnumerable<GetCommentViewModel>>, SelectEntitiesQueryHandler<Comment, GetCommentViewModel>>();
            services.AddScoped<IRequestHandler<SelectEntitiesQuery<Category, MenuViewModel>, IEnumerable<MenuViewModel>>, SelectEntitiesQueryHandler<Category, MenuViewModel>>();
            services.AddScoped<IRequestHandler<SelectEntitiesQuery<Source, SourceFullViewModel>, IEnumerable<SourceFullViewModel>>, SelectEntitiesQueryHandler<Source, SourceFullViewModel>>();
            services.AddScoped<IRequestHandler<UpdateEntityCommand<Comment, UpdateCommentViewModel>, bool>, UpdateEntityCommandHandler<Comment, UpdateCommentViewModel>>();
            services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
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
            app.UseResponseCaching();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "news",
                    pattern: "News/{id:int:min(1)}",
                    defaults: new { controller = "News", action = "Get", });
                endpoints.MapRazorPages();
            });

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, Assembly.Load(COMMANDS_ASSEMBLY));
        }
    }
}