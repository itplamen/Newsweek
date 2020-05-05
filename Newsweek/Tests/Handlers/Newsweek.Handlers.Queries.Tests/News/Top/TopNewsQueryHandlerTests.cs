namespace Newsweek.Handlers.Queries.Tests.News.Top
{
    using System.Linq;
    using System.Threading;
    using System.Reflection;
    using System.Threading.Tasks;
    
    using Xunit;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data;
    using Newsweek.Handlers.Queries.News.Top;
    using Newsweek.Tests.Common.Database;   
    using Newsweek.Web.ViewModels.Common;
    using Newsweek.Web.ViewModels.News;

    public class TopNewsQueryHandlerTests
    {
        private readonly NewsweekDbContext dbContext;
        private readonly TopNewsQueryHandler<NewsBaseViewModel> newsQuery;

        public TopNewsQueryHandlerTests()
        {
            dbContext = InMemoryDbContext.Initialize();
            newsQuery = new TopNewsQueryHandler<NewsBaseViewModel>(dbContext);

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task TopNewsQueryShouldReturnValidResult()
        {
            using (dbContext)
            {
                var query = new TopNewsQuery<NewsBaseViewModel>(1);
                var topNews = await newsQuery.Handle(query, CancellationToken.None);

                Assert.Equal(query.Take, topNews.Count());
                Assert.True(topNews.All(x => !string.IsNullOrWhiteSpace(x.Title)));
                Assert.True(topNews.All(x => !string.IsNullOrWhiteSpace(x.Description)));
                Assert.True(topNews.All(x => x.Subcategory != null));
                Assert.True(topNews.All(x => !string.IsNullOrWhiteSpace(x.Subcategory.Name)));
            }
        }
    }
}