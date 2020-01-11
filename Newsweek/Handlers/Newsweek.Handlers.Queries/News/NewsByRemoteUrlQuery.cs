namespace Newsweek.Handlers.Queries.News
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Newsweek.Handlers.Queries.Contracts;
    using DataNews = Data.Models.News;

    public class NewsByRemoteUrlQuery : IQuery<Task<IEnumerable<DataNews>>>
    {
        public NewsByRemoteUrlQuery(IEnumerable<string> remoteUrls)
        {
            RemoteUrls = remoteUrls;
        }

        public IEnumerable<string> RemoteUrls { get; set; }
    }
}