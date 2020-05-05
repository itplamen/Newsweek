namespace Newsweek.Tests.Common.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    using Newsweek.Data;
    using Newsweek.Data.Models;

    public static class InMemoryDbContext
    {
        public static NewsweekDbContext Initialize()
        {
            var options = new DbContextOptionsBuilder<NewsweekDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new NewsweekDbContext(options);
            dbContext.Users.AddRange(GetUsers());
            dbContext.Sources.AddRange(GetSources());
            dbContext.Categories.AddRange(GetCategories());
            dbContext.Subcategories.AddRange(GetSubcategories());
            dbContext.News.AddRange(GetNews());
            dbContext.Comments.AddRange(GetComments());

            dbContext.SaveChanges();

            return dbContext;
        }

        private static IEnumerable<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    CreatedOn = DateTime.UtcNow,
                    UserName = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    PasswordHash = Guid.NewGuid().ToString()
                }
            };
        }

        private static IEnumerable<Source> GetSources()
        {
            return new List<Source>()
            {
                new Source()
                {
                    CreatedOn = DateTime.UtcNow,
                    Name = Guid.NewGuid().ToString(),
                    Url = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString()
                }
            };
        }

        private static IEnumerable<Category> GetCategories()
        {
            return new List<Category>()
            {
                new Category()
                {
                    CreatedOn = DateTime.UtcNow,
                    Name = Guid.NewGuid().ToString()
                }
            };
        }

        private static IEnumerable<Subcategory> GetSubcategories()
        {
            return new List<Subcategory>()
            {
                new Subcategory()
                {
                    CreatedOn = DateTime.UtcNow,
                    CategoryId = 1,
                    Name = Guid.NewGuid().ToString(),
                }
            };
        } 

        private static IEnumerable<News> GetNews()
        {
            return new List<News>()
            {
                new News()
                {
                    CreatedOn = DateTime.UtcNow,
                    Title = Guid.NewGuid().ToString(),
                    Content = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    MainImageUrl = Guid.NewGuid().ToString(),
                    RemoteUrl = Guid.NewGuid().ToString(),
                    SourceId = 1,
                    SubcategoryId = 1
                }
            };
        }

        private static IEnumerable<Comment> GetComments()
        {
            return new List<Comment>()
            {
                new Comment()
                {
                    CreatedOn = DateTime.UtcNow,
                    Content = Guid.NewGuid().ToString(),
                    NewsId = 1
                }
            };
        }
    }
}