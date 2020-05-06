namespace Newsweek.Handlers.Queries.Tests.Common
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Tests.Common.Database;

    public class GetEntitiesQueryHandlerTests
    {
        private readonly NewsweekDbContext dbContext;
        private readonly GetEntitiesQueryHandler<News> newsQueryHandler;
        private readonly GetEntitiesQueryHandler<Comment, int> commentsQueryHandler;

        public GetEntitiesQueryHandlerTests()
        {
            dbContext = InMemoryDbContext.Initialize();
            newsQueryHandler = new GetEntitiesQueryHandler<News>(dbContext);
            commentsQueryHandler = new GetEntitiesQueryHandler<Comment, int>(dbContext);
        }

        [Fact]
        public async Task GetEntitiesQueryShouldReturnAllNewsWhenNoPredicateIsSpecified()
        {
            var newsQuery = new GetEntitiesQuery<News>();
            var news = await newsQueryHandler.Handle(newsQuery, CancellationToken.None);

            Assert.Equal(dbContext.News.Count(), news.Count());
        }

        [Fact]
        public async Task GetEntitiesQueryShouldReturnNewsThatMeetTheConditionWithSpecifiedPredicate()
        {
            var newsQuery = new GetEntitiesQuery<News>()
            {
                Predicate = x => x.Id > 10
            };

            var news = await newsQueryHandler.Handle(newsQuery, CancellationToken.None);

            Assert.True(news.All(x => x.Id > 10));
        }

        [Fact]
        public async Task GetEntitiesQueryShouldReturnNewsOrderedByIdInDescendingOrder()
        {
            var newsQuery = new GetEntitiesQuery<News>()
            {
                OrderBy = x => x.OrderByDescending(y => y.Id) 
            };

            var news = await newsQueryHandler.Handle(newsQuery, CancellationToken.None);
            var expectedNews = dbContext.News.OrderByDescending(x => x.Id);

            Assert.True(news.SequenceEqual(expectedNews));
        }

        [Theory]
        [InlineData(7)]
        [InlineData(1)]
        [InlineData(13)]
        public async Task GetEntitiesQueryShouldReturnNewsSpecifiedByParameter(int take)
        {
            var newsQuery = new GetEntitiesQuery<News>()
            {
                Take = take
            };

            var news = await newsQueryHandler.Handle(newsQuery, CancellationToken.None);

            Assert.Equal(newsQuery.Take, news.Count());
        }

        [Fact]
        public async Task GetEntitiesQueryShouldReturnSelectedCommentIds()
        {
            var commentsQuery = new GetEntitiesQuery<Comment, int>()
            {
                Selector = x => x.Id
            };

            var commentIds = await commentsQueryHandler.Handle(commentsQuery, CancellationToken.None);

            Assert.Equal(commentIds.Count(), dbContext.Comments.Count());
            Assert.True(commentIds.SequenceEqual(dbContext.Comments.Select(x => x.Id)));
            Assert.True(commentIds.All(dbContext.Comments.Select(x => x.Id).Contains));
        }
    }
}