﻿namespace Newsweek.Data.Seeders
{
    using System.Collections.Generic;

    using Newsweek.Data.Contracts;

    public static class NewsweekDbContextSeeder
    {
        public static void Seed(NewsweekDbContext dbContext)
        {
            IEnumerable<ISeeder> seeders = new List<ISeeder>()
            {
                new SourcesSeeder(),
                new CategoriesSeeder(),
                new RolesSeeder()
            };

            foreach (var seeder in seeders)
            {
                seeder.Seed(dbContext);
            }

            dbContext.SaveChanges();
        }
    }
}