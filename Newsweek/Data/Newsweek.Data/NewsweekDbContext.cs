namespace Newsweek.Data
{
    using Microsoft.EntityFrameworkCore;
    
    using Newsweek.Models;

    public class NewsweekDbContext : DbContext
    {
        public DbSet<Source> Sources { get; set; }

        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) =>
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}