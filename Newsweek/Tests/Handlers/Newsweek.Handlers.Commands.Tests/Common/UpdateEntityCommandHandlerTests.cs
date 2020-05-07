namespace Newsweek.Handlers.Commands.Tests.Common
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common.Update;
    using Newsweek.Tests.Common.Database;
    using Newsweek.Tests.Common.Mapping;
    using Newsweek.Web.ViewModels.Comments;

    public class UpdateEntityCommandHandlerTests
    {
        private readonly NewsweekDbContext dbContext;
        private readonly UpdateEntityCommandHandler<Comment, UpdateCommentViewModel> commentsQueryHandler;

        public UpdateEntityCommandHandlerTests()
        {
            dbContext = InMemoryDbContext.Initialize();
            commentsQueryHandler = new UpdateEntityCommandHandler<Comment, UpdateCommentViewModel>(dbContext);

            MappingConfig.Initialize();
        }

        [Fact]
        public async Task UpdateEntityWithInvalidIdShouldReturnFalseAndNotUpdateAnyEntity()
        {
            var comment = new UpdateCommentViewModel()
            {
                Id = -1,
                Content = $"Test {DateTime.Now.Ticks}"
            };

            var request = new UpdateEntityCommand<Comment, UpdateCommentViewModel>(comment);
            var isUpdated = await commentsQueryHandler.Handle(request, CancellationToken.None);

            Assert.False(isUpdated);
            Assert.Null(dbContext.Comments.FirstOrDefault(x => x.Id == comment.Id));
        }

        [Fact]
        public async Task UpdateEntityWithInvalidIdShouldReturnTrueAndUpdateTheEntity()
        {
            var comment = new UpdateCommentViewModel()
            {
                Id = dbContext.Comments.First().Id,
                Content = $"Test {DateTime.Now.Ticks}"
            };

            var request = new UpdateEntityCommand<Comment, UpdateCommentViewModel>(comment);
            var isUpdated = await commentsQueryHandler.Handle(request, CancellationToken.None);

            Assert.True(isUpdated);
            Assert.Equal(dbContext.Comments.First(x => x.Id == comment.Id).Content, comment.Content);
            Assert.True(dbContext.Comments.First(x => x.Id == comment.Id).ModifiedOn != null);
        }
    }
}