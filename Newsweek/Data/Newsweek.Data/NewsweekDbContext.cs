﻿namespace Newsweek.Data
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

        public DbSet<News> News { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) =>
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}