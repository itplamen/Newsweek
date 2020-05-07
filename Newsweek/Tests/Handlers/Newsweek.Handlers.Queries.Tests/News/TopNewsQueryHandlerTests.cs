namespace Newsweek.Handlers.Queries.Tests.News
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using Xunit;

    using Newsweek.Common.Constants;
    using Newsweek.Data;
    using Newsweek.Handlers.Queries.News.Top;
    using Newsweek.Tests.Common.Database;   
    using Newsweek.Tests.Common.Mapping;
    using Newsweek.Web.ViewModels.News;

    public class TopNewsQueryHandlerTests
    {
        private readonly NewsweekDbContext dbContext;
        private readonly TopNewsQueryHandler<NewsBaseViewModel> newsQueryHandler;

        public TopNewsQueryHandlerTests()
        {
            dbContext = InMemoryDbContext.Initialize();
            newsQueryHandler = new TopNewsQueryHandler<NewsBaseViewModel>(dbContext);

            MappingConfig.Initialize();
        }

        [Fact]
        public async Task TopNewsQueryShouldReturnValidResult()
        {
            using (dbContext)
            {
                var query = new TopNewsQuery<NewsBaseViewModel>();
                var topNews = await newsQueryHandler.Handle(query, CancellationToken.None);
                var expectedCount = dbContext.Categories.Count() * PublicConstants.TAKE_NEWS_PER_CATEGORY;

                Assert.Equal(expectedCount, topNews.Count());
                Assert.True(topNews.All(x => !string.IsNullOrWhiteSpace(x.Title)));
                Assert.True(topNews.All(x => !string.IsNullOrWhiteSpace(x.Description)));
                Assert.True(topNews.All(x => x.Subcategory != null));
                Assert.True(topNews.All(x => !string.IsNullOrWhiteSpace(x.Subcategory.Name)));
            }
        }
    }
}