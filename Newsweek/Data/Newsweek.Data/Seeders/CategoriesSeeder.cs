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
            IEnumerable<string> categoryNames = new List<string>()
            {
               "Europe", "World", "Sport"
            };

            foreach (var name in categoryNames)
            {
                if (!dbContext.Categories.Any(x => x.Name == name))
                {
                    Category category = new Category()
                    {
                        Name = name,
                        CreatedOn = DateTime.UtcNow
                    };

                    dbContext.Categories.Add(category);
                }
            }
        }
    }
}