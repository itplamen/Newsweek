using AngleSharp.Dom;
using Newsweek.Worker.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Newsweek.Worker.Core.Providers
{
    public class EuronewsProvider : INewsProvider
    {
        private const string URL = "https://www.euronews.com/news/europe";

        private readonly INewsApi newsApi;

        public EuronewsProvider(INewsApi newsApi)
        {
            this.newsApi = newsApi;
        }

        public async Task Get()
        {
            IDocument document = await newsApi.Get(URL);

            IHtmlCollection<IElement> elements = document.QuerySelectorAll(".m-object--demi");
        }
    }
}
