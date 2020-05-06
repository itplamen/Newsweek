namespace Newsweek.Handlers.Queries.Tests.News.Search
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    using Newsweek.Data;
    using Newsweek.Handlers.Queries.News.Search;
    using Newsweek.Tests.Common.Database;

    public class SearchNewsQueryHandlerTests
    {
        private readonly NewsweekDbContext dbContext;
        private readonly SearchNewsQueryHandler searchNewsQueryHandler;

        public SearchNewsQueryHandlerTests()
        {
            dbContext = InMemoryDbContext.Initialize();
            searchNewsQueryHandler = new SearchNewsQueryHandler(dbContext);
        }

        [Fact]
        public async Task SearchNewsQueryWithoutSearchCriteriaShouldReturnAllNewsWithThreeNewsPerPage()
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                NewsPerPage = 3
            };
            
            var response = await searchNewsQueryHandler.Handle(searchNewsQuery, CancellationToken.None);

            Assert.Equal(dbContext.News.Count(), response.NewsCount);
            Assert.Equal(searchNewsQuery.NewsPerPage, response.News.Count());
            Assert.True(string.IsNullOrEmpty(response.Search.Category));
            Assert.True(string.IsNullOrEmpty(response.Search.Subcategory));
        }

        [Fact]
        public async Task SearchNewsQueryShouldReturnNewsWhenSearchingByExistingTag()
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                Tag = "test 1",
                NewsPerPage = 20
            };

            var response = await searchNewsQueryHandler.Handle(searchNewsQuery, CancellationToken.None);

            Assert.True(response.NewsCount > 0);
            Assert.Equal(searchNewsQuery.Tag, response.Search.Category);
            Assert.True(string.IsNullOrEmpty(response.Search.Subcategory));
        }

        [Fact]
        public async Task SearchNewsQueryShouldReturnNewsWhenSearchingByMissingTag()
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                Tag = Guid.NewGuid().ToString(),
                NewsPerPage = 20
            };

            var response = await searchNewsQueryHandler.Handle(searchNewsQuery, CancellationToken.None);

            Assert.Equal(0, response.NewsCount);
            Assert.Equal(searchNewsQuery.Tag, response.Search.Category);
            Assert.True(string.IsNullOrEmpty(response.Search.Subcategory));
        }

        [Fact]
        public async Task SearchNewsQueryShouldReturnNewsWhenSearchingByExistingCategory()
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                Category = "Test 1",
                NewsPerPage = 20
            };

            var response = await searchNewsQueryHandler.Handle(searchNewsQuery, CancellationToken.None);

            Assert.True(response.NewsCount > 0);
            Assert.Equal(searchNewsQuery.Category, response.Search.Category);
            Assert.True(string.IsNullOrEmpty(response.Search.Subcategory));
        }

        [Fact]
        public async Task SearchNewsQueryShouldReturnNewsWhenSearchingByMissingCategory()
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                Category = Guid.NewGuid().ToString(),
                NewsPerPage = 20
            };

            var response = await searchNewsQueryHandler.Handle(searchNewsQuery, CancellationToken.None);

            Assert.Equal(0, response.NewsCount);
            Assert.Equal(searchNewsQuery.Category, response.Search.Category);
            Assert.True(string.IsNullOrEmpty(response.Search.Subcategory));
        }

        [Fact]
        public async Task SearchNewsQueryShouldReturnNewsWhenSearchingByExistingSubcategory()
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                Category = "Test 1",
                Subcategory = "Test 1",
                NewsPerPage = 20
            };

            var response = await searchNewsQueryHandler.Handle(searchNewsQuery, CancellationToken.None);

            Assert.True(response.NewsCount > 0);
            Assert.Equal(searchNewsQuery.Category, response.Search.Category);
            Assert.Equal(searchNewsQuery.Subcategory, response.Search.Subcategory);
        }

        [Fact]
        public async Task SearchNewsQueryShouldReturnNewsWhenSearchingByMissingSubcategory()
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                Category = "Test 1",
                Subcategory = Guid.NewGuid().ToString(),
                NewsPerPage = 20
            };

            var response = await searchNewsQueryHandler.Handle(searchNewsQuery, CancellationToken.None);

            Assert.Equal(0, response.NewsCount);
            Assert.Equal(searchNewsQuery.Category, response.Search.Category);
            Assert.Equal(searchNewsQuery.Subcategory, response.Search.Subcategory);
        }

        [Fact]
        public async Task SearchNewsQueryShouldNotReturnCurrentPageWhenCouldNotFindAnyNews()
        {
            var searchNewsQuery = new SearchNewsQuery()
            {
                Tag = Guid.NewGuid().ToString(),
                NewsPerPage = 20
            };

            var response = await searchNewsQueryHandler.Handle(searchNewsQuery, CancellationToken.None);

            Assert.Equal(0, response.NewsCount);
            Assert.Equal(0, response.CurrentPage);
        }
    }
}