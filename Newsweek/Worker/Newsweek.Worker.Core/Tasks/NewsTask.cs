namespace Newsweek.Worker.Core.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Worker.Core.Contracts;

    public class NewsTask : ITask
    {
        private readonly IEnumerable<INewsProvider> newsProviders;
        private readonly ICommandHandler<CreateNewsCommand> newsHandler;

        public NewsTask(IEnumerable<INewsProvider> newsProviders, ICommandHandler<CreateNewsCommand> newsHandler)
        {
            this.newsProviders = newsProviders;
            this.newsHandler = newsHandler;
        }

        public async Task DoWork()
        {
            var newsCommands = await Task.WhenAll(newsProviders.Select(x => x.Get()));

            foreach (var news in newsCommands.SelectMany(x => x))
            {
                newsHandler.Handle(news);
            }
        }
    }
}