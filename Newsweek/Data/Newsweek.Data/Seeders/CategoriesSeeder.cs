namespace Newsweek.Data.Seeders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newsweek.Data.Contracts;
    using Newsweek.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public void Seed(NewsweekDbContext dbContext)
        {
            IEnumerable<Category> categories = new List<Category>()
            {
                new Category() { Name = "Europe" },
                new Category() { Name = "IT" },
                new Category()
                {
                    Name = "Sport",
                    Subcategories = new List<Subcategory>()
                    {
                        new Subcategory() { Name = "Football" },
                        new Subcategory() { Name = "Boxing" },
                        new Subcategory() { Name = "Tennis" }
                    }
                },
                new Category()
                {
                    Name = "World",
                    Subcategories = new List<Subcategory>()
                    {
                        new Subcategory() { Name = "US & Canada" },
                        new Subcategory() { Name = "Asia" },
                        new Subcategory() { Name = "Middle East" }
                    }
                }
            };

            foreach (var category in categories)
            {
                if (!dbContext.Categories.Any(x => x.Name == category.Name))
                {
                    category.CreatedOn = DateTime.UtcNow;

                    foreach (var subcategory in category.Subcategories)
                    {
                        subcategory.CreatedOn = DateTime.UtcNow;
                    }

                    dbContext.Categories.Add(category);
                }
            }
        }
    }
}