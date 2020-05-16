namespace Newsweek.Worker.Core.Tests.Providers
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    using Newsweek.Data.Models;
    using Newsweek.Tests.Common.Mocks;
    using Newsweek.Worker.Core.Contracts;
    using Newsweek.Worker.Core.Providers;

    public class EuropeNewsProviderTests
    {
        private readonly Source source;
        private readonly Category category;
        private readonly INewsProvider newsProvider;

        public EuropeNewsProviderTests()
        {
            this.source = new Source()
            {
                Name = "Euronews",
                Url = "https://www.euronews.com"
            };

            this.category = new Category()
            {
                Name = "Europe"
            };

            this.newsProvider = new EuropeNewsProvider(NewsApiMock.GetNewsApi());
        }

        [Fact]
        public async Task EuropeNewsProviderWithMissingSourceShouldThrowArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => newsProvider.Get(null, category));
        }

        [Fact]
        public async Task EuropeNewsProviderWithMissingCategoryShouldThrowArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => newsProvider.Get(source, null));
        }
    }
}