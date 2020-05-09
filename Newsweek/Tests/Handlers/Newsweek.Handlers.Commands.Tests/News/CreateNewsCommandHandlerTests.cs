namespace Newsweek.Handlers.Commands.Tests.News
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Xunit;

    using Newsweek.Handlers.Commands.News;
    using Newsweek.Tests.Common.Constants;
    using Newsweek.Tests.Common.Mocks;

    public class CreateNewsCommandHandlerTests
    {
        private readonly IMediator mediator;
        private readonly CreateNewsCommandHandler newsCommandHandler;

        public CreateNewsCommandHandlerTests()
        {
            mediator = MediatorMock.GetMediator();
            newsCommandHandler = new CreateNewsCommandHandler(mediator);
        }

        [Fact]
        public async Task CreateNewsShouldNotReturnAnyNewsWhenRequestIsEmpty()
        {
            var request = new CreateNewsCommand(Enumerable.Empty<NewsCommand>());
            var createdNews = await newsCommandHandler.Handle(request, CancellationToken.None);

            Assert.False(createdNews.Any());
        }

        [Fact]
        public async Task CreateNewsShouldNotReturnAnyNewsWhenTryingToCreateEntityWithExistingRemoteUrl()
        {
            var newsCommands = new List<NewsCommand>()
            {
                new NewsCommand()
                {
                    RemoteUrl = TestConstants.EXISTING_PROPERTY_FOR_ENTITY
                }
            };

            var request = new CreateNewsCommand(newsCommands);
            var createdNews = await newsCommandHandler.Handle(request, CancellationToken.None);

            Assert.False(createdNews.Any());
        }

        [Fact]
        public async Task CreateNewsShouldReturnCollectionWithCreatedEntityWhenTryingCreateNewsWithValidRemoteUrl()
        {
            var newsCommands = new List<NewsCommand>()
            {
                new NewsCommand()
                {
                    RemoteUrl = TestConstants.VALID_PROPERTY_FOR_ENTITY
                }
            };

            var request = new CreateNewsCommand(newsCommands);
            var createdNews = await newsCommandHandler.Handle(request, CancellationToken.None);

            Assert.True(createdNews.Any());
            Assert.Equal(newsCommands.Count, createdNews.Count());
            Assert.Equal(newsCommands.First().RemoteUrl, createdNews.First().RemoteUrl);
            Assert.True(createdNews.First().Id > 0);
            Assert.True(createdNews.First().CreatedOn != default);
        }
    }
}