namespace Newsweek.Handlers.Commands.News
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using MediatR;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Queries.Common;

    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, IEnumerable<Data.Models.News>>
    {
        private readonly IMediator mediator; 

        public CreateNewsCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<Data.Models.News>> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            GetEntitiesQuery<News> newsQuery = new GetEntitiesQuery<News>()
            {
                Predicate = x => request.News.Select(x => x.RemoteUrl).Contains(x.RemoteUrl)
            };

            IEnumerable<News> news = await mediator.Send(newsQuery, cancellationToken);

            ICollection<NewsCommand> newsCommandsToCreate = new List<NewsCommand>();

            foreach (var newsCommand in request.News)
            {
                if (news == null || !news.Any(x => x.RemoteUrl == newsCommand.RemoteUrl))
                {
                    newsCommandsToCreate.Add(newsCommand);
                }
            }

            return await mediator.Send(new CreateEntitiesCommand<News>(newsCommandsToCreate), cancellationToken);
        }
    }
}