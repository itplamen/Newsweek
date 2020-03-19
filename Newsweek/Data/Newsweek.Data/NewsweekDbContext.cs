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

        public DbSet<Category> Categories { get; set; }

        public DbSet<Email> Emails { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<NewsTag> NewsTags { get; set; }

        public DbSet<Source> Sources { get; set; }

        public DbSet<Subcategory> Subcategories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<UserEmail> UserEmails { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) =>
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}