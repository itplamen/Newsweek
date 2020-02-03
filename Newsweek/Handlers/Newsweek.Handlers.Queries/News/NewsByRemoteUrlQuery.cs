namespace Newsweek.Handlers.Queries.News
{
    using System.Collections.Generic;

    using Newsweek.Handlers.Queries.Contracts;
    using DataNews = Data.Models.News;

    public class NewsByRemoteUrlQuery : IQuery<IEnumerable<DataNews>>
    {
        public NewsByRemoteUrlQuery(IEnumerable<string> remoteUrls)
        {
            RemoteUrls = remoteUrls;
        }

        public IEnumerable<string> RemoteUrls { get; set; }
    }
}