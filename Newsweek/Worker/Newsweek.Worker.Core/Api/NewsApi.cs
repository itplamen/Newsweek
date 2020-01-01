namespace Newsweek.Worker.Core.Api
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Parser;
    
    using Newsweek.Worker.Core.Contracts;

    public class NewsApi : INewsApi
    {
        private readonly HttpClient httpClient;
        private readonly IHtmlParser htmlParser;

        public NewsApi(IHttpClientFactory httpClientFactory, IHtmlParser htmlParser)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.htmlParser = htmlParser;
        }

        public async Task<IDocument> Get(string url)
        {
            string html = await httpClient.GetStringAsync(url);
            IHtmlDocument document = htmlParser.ParseDocument(html);

            return document;
        }
    }
}