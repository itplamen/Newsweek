namespace Newsweek.Handlers.Commands.News
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;

    public class CreateNewsCommandHandler : ICommandHandler<CreateNewsCommand>
    {
        private readonly IQueryHandler<GetEntitiesQuery<News>, IEnumerable<News>> newsGetHandler;
        private readonly ICommandHandler<CreateEntitiesCommand<News, int>, IEnumerable<News>> newsCreateHandler;

        public CreateNewsCommandHandler(
            IQueryHandler<GetEntitiesQuery<News>, IEnumerable<News>> newsGetHandler, 
            ICommandHandler<CreateEntitiesCommand<News, int>, IEnumerable<News>> newsCreateHandler)
        {
            this.newsGetHandler = newsGetHandler;
            this.newsCreateHandler = newsCreateHandler;
        }

        public async Task Handle(CreateNewsCommand command)
        {
            Expression<Func<News, bool>> newsFilter = x => command.News.Select(x => x.RemoteUrl).Contains(x.RemoteUrl);
            IEnumerable<News> news = await newsGetHandler.Handle(new GetEntitiesQuery<News>(newsFilter));

            ICollection<NewsCommand> newsCommandsToCreate = new List<NewsCommand>();

            foreach (var newsCommand in command.News)
            {
                if (news == null || !news.Any(x => x.RemoteUrl == newsCommand.RemoteUrl))
                {
                    newsCommandsToCreate.Add(newsCommand);
                }
            }

            await newsCreateHandler.Handle(new CreateEntitiesCommand<News, int>(newsCommandsToCreate));
        }
    }
}