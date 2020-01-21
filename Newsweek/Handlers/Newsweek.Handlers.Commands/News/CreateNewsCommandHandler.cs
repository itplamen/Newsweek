namespace Newsweek.Handlers.Commands.News
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Queries.Contracts;
    using Newsweek.Handlers.Queries.News;

    public class CreateNewsCommandHandler : ICommandHandler<CreateNewsCommand, Task<IEnumerable<News>>>
    {
        private readonly IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<News, int>, Task<IEnumerable<News>>> newsCreateHandler;

        public CreateNewsCommandHandler(
            IQueryHandler<NewsByRemoteUrlQuery, Task<IEnumerable<News>>> newsGetHandler, 
            ICommandHandler<CreateEntitiesCommand<News, int>, Task<IEnumerable<News>>> newsCreateHandler)
        {
            this.newsGetHandler = newsGetHandler;
            this.newsCreateHandler = newsCreateHandler;
        }

        public async Task<IEnumerable<News>> Handle(CreateNewsCommand command)
        {
            ICollection<NewsCommand> newsCommandsToCreate = new List<NewsCommand>();

            IEnumerable<string> urls = command.News.Select(x => x.RemoteUrl);
            IEnumerable<News> news = await newsGetHandler.Handle(new NewsByRemoteUrlQuery(urls));

            foreach (var newsCommand in command.News)
            {
                if (news == null || !news.Any(x => x.RemoteUrl == newsCommand.RemoteUrl))
                {
                    newsCommandsToCreate.Add(newsCommand);
                }
            }

            return await newsCreateHandler.Handle(new CreateEntitiesCommand<News, int>(newsCommandsToCreate));
        }
    }
}