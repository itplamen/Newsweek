namespace Newsweek.Handlers.Commands.News
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    using MediatR;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Commands.Contracts;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;

    public class CreateNewsCommandHandler : ICommandHandler<CreateNewsCommand>
    {
        private readonly IMediator mediator; 

        private readonly IQueryHandler<GetEntitiesQuery<News>, IEnumerable<News>> newsGetHandler;

        public CreateNewsCommandHandler(IMediator mediator, IQueryHandler<GetEntitiesQuery<News>, IEnumerable<News>> newsGetHandler)
        {
            this.mediator = mediator;
            this.newsGetHandler = newsGetHandler;
        }

        public async Task Handle(CreateNewsCommand command)
        {
            GetEntitiesQuery<News> newsQuery = new GetEntitiesQuery<News>()
            {
                Predicate =  x => command.News.Select(x => x.RemoteUrl).Contains(x.RemoteUrl)
            };

            IEnumerable<News> news = await newsGetHandler.Handle(newsQuery);

            ICollection<NewsCommand> newsCommandsToCreate = new List<NewsCommand>();

            foreach (var newsCommand in command.News)
            {
                if (news == null || !news.Any(x => x.RemoteUrl == newsCommand.RemoteUrl))
                {
                    newsCommandsToCreate.Add(newsCommand);
                }
            }

            await mediator.Send(new CreateEntitiesCommand<News, int>(newsCommandsToCreate));
        }
    }
}