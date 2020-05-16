namespace Newsweek.Tests.Common.Mocks
{
    using System.Threading.Tasks;

    using AngleSharp.Dom;

    using Moq;

    using Newsweek.Worker.Core.Contracts;
    
    public static class NewsApiMock
    {
        public static INewsApi GetNewsApi()
        {
            var newsApi = new Mock<INewsApi>();

            newsApi.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(Task.FromResult<IDocument>(null));

            return newsApi.Object;
        }
    }
}