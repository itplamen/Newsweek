namespace Newsweek.Handlers.Commands.Tests.Common
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common.Delete;
    using Newsweek.Tests.Common.Database;

    public class DeleteEntityCommandHandlerTests
    {
        private readonly NewsweekDbContext dbContext;
        private readonly DeleteEntityCommandHandler<News> newsQueryHandler;

        public DeleteEntityCommandHandlerTests()
        {
            dbContext = InMemoryDbContext.Initialize();
            newsQueryHandler = new DeleteEntityCommandHandler<News>(dbContext);
        }

        [Fact]
        public async Task DeleteEntityWithInvalidIdShouldReturnFalseAndNotDeleteAnyEntity()
        {
            var id = -1;
            var request = new DeleteEntityCommand<News>(id);
            var isDeleted = await newsQueryHandler.Handle(request, CancellationToken.None);

            Assert.False(isDeleted);
            Assert.Null(dbContext.News.FirstOrDefault(x => x.Id == id));
        }

        [Fact]
        public async Task DeleteEntityWithValidIdShouldReturnTrueAndDeleteTheEntity()
        {
            var id = dbContext.News.First().Id;
            var request = new DeleteEntityCommand<News>(id);
            var isDeleted = await newsQueryHandler.Handle(request, CancellationToken.None);

            Assert.True(isDeleted);
            Assert.True(dbContext.News.First(x => x.Id == id).IsDeleted);
            Assert.True(dbContext.News.First(x => x.Id == id).DeletedOn != null);
        }
    }
}