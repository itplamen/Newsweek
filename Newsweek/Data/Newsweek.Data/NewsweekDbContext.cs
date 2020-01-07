namespace Newsweek.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    
    using Newsweek.Data.Models;

    public class NewsweekDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public NewsweekDbContext(DbContextOptions<NewsweekDbContext> options)
            : base(options)
        {
        }

        public DbSet<Source> Sources { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Subcategory> Subcategories { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<NewsSubcategory> NewsCategory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) =>
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}