namespace Newsweek.Handlers.Commands.Tests.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Xunit;

    using Newsweek.Common.Constants;
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common.Create;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Tests.Common.Database;

    public class CreateEntitiesCommandHandlerTests
    {
        private readonly NewsweekDbContext dbContext;
        private readonly CreateEntitiesCommandHandler<News> newsQueryHandler;

        public CreateEntitiesCommandHandlerTests()
        {
            dbContext = InMemoryDbContext.Initialize();
            newsQueryHandler = new CreateEntitiesCommandHandler<News>(dbContext);

            AutoMapperConfig.RegisterMappings(Assembly.Load(PublicConstants.COMMANDS_ASSEMBLY));
        }

        [Fact]
        public async Task CreateEntitiesShouldReturnEmptyCollectionWhenRequestEntitiesAreNull()
        {
            var request = new CreateEntitiesCommand<News>(null);
            var createdNews = await newsQueryHandler.Handle(request, CancellationToken.None);

            Assert.False(createdNews.Any());
        }

        [Fact]
        public async Task CreateEntitiesShouldReturnEmptyCollectionWhenRequestEntitiesAreEmpty()
        {
            var request = new CreateEntitiesCommand<News>(Enumerable.Empty<IRequest>());
            var createdNews = await newsQueryHandler.Handle(request, CancellationToken.None);

            Assert.False(createdNews.Any());
        }

        [Fact]
        public async Task CreateEntitiesShouldReturnCollectionWithCreatedNews()
        {
            var newsCount = dbContext.News.Count();
            var lastNewsId = dbContext.News.Last().Id;

            var entities = new List<NewsCommand>()
            {
                new NewsCommand()
                {
                    Title = "TestTitle",
                    Content = "TestContent",
                    RemoteUrl = "TestRemoteUrl",
                    Description = "TestDescription",
                    SourceId = 1,
                    SubcategoryId = 1
                }
            };

            var request = new CreateEntitiesCommand<News>(entities);
            var createdNews = await newsQueryHandler.Handle(request, CancellationToken.None);

            Assert.Equal(entities.Count(), createdNews.Count());
            Assert.True(createdNews.First().Id > 0);
            Assert.Equal(entities.First().Title, createdNews.First().Title);
            Assert.Equal(entities.First().Content, createdNews.First().Content);
            Assert.Equal(entities.First().RemoteUrl, createdNews.First().RemoteUrl);
            Assert.Equal(entities.First().Description, createdNews.First().Description);
            Assert.Equal(entities.First().SourceId, createdNews.First().SourceId);
            Assert.Equal(entities.First().SubcategoryId, createdNews.First().SubcategoryId);
            Assert.Equal(newsCount + 1, dbContext.News.Count());
            Assert.Equal(lastNewsId + 1, createdNews.First().Id);
        }
    }
}