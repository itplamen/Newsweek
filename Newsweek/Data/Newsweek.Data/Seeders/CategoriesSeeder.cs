namespace Newsweek.Data.Seeders
{
    using System;
    using System.Collections.Generic;

    using Newsweek.Data.Contracts;
    using Newsweek.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public void Seed(NewsweekDbContext dbContext)
        {
            IEnumerable<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Europe",
                    CreatedOn = DateTime.UtcNow
                }
            };

            foreach (var category in categories)
            {
                dbContext.Categories.Add(category);
            }
        }
    }
}