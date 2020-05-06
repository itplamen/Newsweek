namespace Newsweek.Tests.Common.Database
{
    using System;
    using System.Collections.Generic;

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
            dbContext.Tags.AddRange(GetTags());
            dbContext.NewsTags.AddRange(GetNewsTags());

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
            var categoriesList = new List<Category>();

            for (int i = 1; i <= 4; i++)
            {
                var category = new Category()
                {
                    CreatedOn = DateTime.UtcNow,
                    Name = $"Test {i}"
                };

                categoriesList.Add(category);
            }

            return categoriesList;
        }

        private static IEnumerable<Subcategory> GetSubcategories()
        {
            var subcategoriesList = new List<Subcategory>();

            for (int i = 1; i <= 20; i++)
            {
                int categoryId = i % 5;

                if (categoryId == 0)
                {
                    categoryId = 1;
                }

                var subcategory = new Subcategory()
                {
                    CreatedOn = DateTime.UtcNow,
                    CategoryId = categoryId,
                    Name = $"Test {i}"
                };

                subcategoriesList.Add(subcategory);
            }

            return subcategoriesList;
        } 

        private static IEnumerable<News> GetNews()
        {
            var newsList = new List<News>();

            for (int i = 1; i <= 20; i++)
            {
                var news = new News()
                {
                    CreatedOn = DateTime.UtcNow,
                    Title = Guid.NewGuid().ToString(),
                    Content = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    MainImageUrl = Guid.NewGuid().ToString(),
                    RemoteUrl = Guid.NewGuid().ToString(),
                    SourceId = 1,
                    SubcategoryId = i
                };

                newsList.Add(news);
            }

            return newsList;
        }

        private static IEnumerable<Comment> GetComments()
        {
            var commentsList = new List<Comment>();

            for (int i = 1; i <= 100; i++)
            {
                int newsId = i % 20;

                if (newsId == 0)
                {
                    newsId = 1;
                }

                var comment = new Comment()
                {
                    CreatedOn = DateTime.UtcNow,
                    Content = Guid.NewGuid().ToString(),
                    NewsId = newsId
                };

                commentsList.Add(comment);
            }

            return commentsList;
        }

        private static IEnumerable<Tag> GetTags()
        {
            var tagsList = new List<Tag>();

            for (int i = 1; i <= 20; i++)
            {
                var tag = new Tag()
                {
                    CreatedOn = DateTime.UtcNow,
                    Name = $"Test {i}"
                };

                tagsList.Add(tag);
            }

            return tagsList;
        }

        private static IEnumerable<NewsTag> GetNewsTags()
        {
            var newsTagsList = new List<NewsTag>();

            for (int i = 1; i <= 20; i++)
            {
                var newsTag = new NewsTag()
                {
                    CreatedOn = DateTime.UtcNow,
                    NewsId = i,
                    TagId = i
                };

                newsTagsList.Add(newsTag);
            }

            return newsTagsList;
        }
    }
}