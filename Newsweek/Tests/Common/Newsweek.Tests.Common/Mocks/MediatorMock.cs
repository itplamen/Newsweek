namespace Newsweek.Tests.Common.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common.Create;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Tests.Common.Constants;

    public static class MediatorMock
    {
        public static IMediator GetMediator()
        {
            var mediator = new Mock<IMediator>();

            var existingNews = new News()
            {
                RemoteUrl = TestConstants.EXISTING_NEWS_REMOTE_URL
            };

            var newlyCreatedNews = new News()
            {
                Id = int.MaxValue,
                CreatedOn = DateTime.UtcNow,
                RemoteUrl = TestConstants.VALID_NEWS_REMOTE_URL
            };

            mediator.Setup(x => x.Send(
                    It.Is<GetEntitiesQuery<News>>(y => y.Predicate.Compile() (existingNews)), 
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<News>>(new List<News>() { existingNews }));

            mediator.Setup(x => x.Send(
                    It.Is<GetEntitiesQuery<News>>(y => y.Predicate.Compile() (newlyCreatedNews)),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Enumerable.Empty<News>()));

            mediator.Setup(x => x.Send(
                    It.Is<CreateEntitiesCommand<News>>(y => !y.Entities.Any()),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Enumerable.Empty<News>()));

            mediator.Setup(x => x.Send(
                    It.Is<CreateEntitiesCommand<News>>(y => y.Entities.Any()),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IEnumerable<News>>(new List<News>() { newlyCreatedNews }));

            return mediator.Object;
        }
    }
}