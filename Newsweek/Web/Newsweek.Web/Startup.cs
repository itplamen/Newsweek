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
    using Newsweek.Common.Infrastructure.Logging;

    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Data.Seeders;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.News;
    using Newsweek.Handlers.Commands.Common.Update;
    using Newsweek.Handlers.Queries.News.Search;
    using Newsweek.Web.Models.Comments;
    using Newsweek.Web.Models.Common;
    using Newsweek.Web.Models.Menu;
    using Newsweek.Web.Models.News;
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
            services.AddScoped<IRequestHandler<GetEntitiesQuery<News, NewsBaseViewModel>, IEnumerable<NewsBaseViewModel>>, GetEntitiesQueryHandler<News, NewsBaseViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<News, NewsViewModel>, IEnumerable<NewsViewModel>>, GetEntitiesQueryHandler<News, NewsViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Comment, GetCommentViewModel>, IEnumerable<GetCommentViewModel>>, GetEntitiesQueryHandler<Comment, GetCommentViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Category, MenuViewModel>, IEnumerable<MenuViewModel>>, GetEntitiesQueryHandler<Category, MenuViewModel>>();
            services.AddScoped<IRequestHandler<GetEntitiesQuery<Source, SourceFullViewModel>, IEnumerable<SourceFullViewModel>>, GetEntitiesQueryHandler<Source, SourceFullViewModel>>();
            services.AddScoped<IRequestHandler<UpdateEntityCommand<Comment, UpdateCommentViewModel>, bool>, UpdateEntityCommandHandler<Comment, UpdateCommentViewModel>>();
            services.AddScoped<IRequestHandler<SearchNewsQuery, SearchNewsResponse>, SearchNewsQueryHandler>();
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
            });

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, Assembly.Load(COMMANDS_ASSEMBLY));
        }
    }
}