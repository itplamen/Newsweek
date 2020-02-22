namespace Newsweek.Handlers.Commands.NewsTags
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
    
    public class CreateNewsTagsCommandHandler : IRequestHandler<CreateNewsTagsCommand>
    {
        private readonly IMediator mediator;

        public CreateNewsTagsCommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(CreateNewsTagsCommand request, CancellationToken cancellationToken)
        {
            ICollection<NewsTagCommand> newsTagCommands = new List<NewsTagCommand>();

            foreach (var newsCommand in request.NewsCommands)
            {
                int? newsId = request.News.FirstOrDefault(x => x.RemoteUrl == newsCommand.RemoteUrl)?.Id;

                if (newsId.HasValue)
                {
                    foreach (var tagCommand in newsCommand.Tags)
                    {
                        int? tagId = request.Tags.FirstOrDefault(x => x.Name == tagCommand.Name)?.Id;

                        if (tagId.HasValue)
                        {
                            newsTagCommands.Add(new NewsTagCommand(newsId.Value, tagId.Value));
                        }
                    }
                }
            }

            GetEntitiesQuery<NewsTag> newsTagsQuery = new GetEntitiesQuery<NewsTag>()
            {
                Predicate = x => newsTagCommands.Select(y => y.NewsId).Contains(x.NewsId) && 
                    newsTagCommands.Select(y => y.TagId).Contains(x.TagId)
            };

            IEnumerable<NewsTag> existingNewsTags = await mediator.Send(newsTagsQuery, cancellationToken);

            IEnumerable<NewsTagCommand> newsTagCommandsToCreate = newsTagCommands.Where(x =>
                !existingNewsTags.Any(y => y.NewsId == x.NewsId) && 
                !existingNewsTags.Any(y => y.TagId == x.TagId));

            await mediator.Send(new CreateEntitiesCommand<NewsTag>(newsTagCommandsToCreate), cancellationToken);

            return Unit.Value;
        }
    }
}