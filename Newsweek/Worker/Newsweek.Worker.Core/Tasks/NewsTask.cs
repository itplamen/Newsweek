namespace Newsweek.Worker.Core.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Handlers.Queries.News;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly IEnumerable<INewsProvider> newsProviders;
        private readonly ICommandHandler<CreateNewsCommand> newsCreateHandler;
        private readonly IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler;

        public NewsTask(
            IEnumerable<INewsProvider> newsProviders, 
            ICommandHandler<CreateNewsCommand> newsCreateHandler,
            IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler)
        {
            this.newsProviders = newsProviders;
            this.newsCreateHandler = newsCreateHandler;
            this.newsGetHandler = newsGetHandler;
        }

        public async Task DoWork()
        {
            IEnumerable<CreateNewsCommand>[] providedNewsCommands = await Task.WhenAll(newsProviders.Select(x => x.Get()));
            IEnumerable<CreateNewsCommand> newsCommands = providedNewsCommands.SelectMany(x => x);

            IEnumerable<string> urls = newsCommands.Select(x => x.RemoteUrl);
            IEnumerable<News> news = await newsGetHandler.Handle(new NewsByRemoteUrlQuery(urls));

            foreach (var command in newsCommands)
            {
                if (news == null || !news.Any(x => x.RemoteUrl == command.RemoteUrl))
                {
                    newsCreateHandler.Handle(command);
                }
            }
        }
    }
}