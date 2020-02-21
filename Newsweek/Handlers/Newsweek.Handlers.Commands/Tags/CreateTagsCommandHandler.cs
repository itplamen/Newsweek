namespace Newsweek.Handlers.Commands.Tags
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using MediatR;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Common;
    using Newsweek.Handlers.Queries.Common;
    using Newsweek.Handlers.Queries.Contracts;

    public class CreateTagsCommandHandler : IRequestHandler<CreateTagsCommand>
    {
        private readonly IMediator mediator;
        private readonly IQueryHandler<GetEntitiesQuery<Tag>, IEnumerable<Tag>> getHandler;

        public CreateTagsCommandHandler(Mediator mediator, IQueryHandler<GetEntitiesQuery<Tag>, IEnumerable<Tag>> getHandler)
        {
            this.mediator = mediator;
            this.getHandler = getHandler;
        }

        public async Task<Unit> Handle(CreateTagsCommand request, CancellationToken cancellationToken)
        {
            request.Tags = request.Tags
                .GroupBy(x => x.Name)
                .Where(x => x.Count() == 1 && !string.IsNullOrEmpty(x.Key))
                .SelectMany(x => x);

            var query = new GetEntitiesQuery<Tag>()
            {
                Predicate = x => request.Tags
                    .Select(x => x.Name)
                    .Contains(x.Name)
            };

            IEnumerable<Tag> tags = await getHandler.Handle(query);

            ICollection<TagCommand> tagsToCreate = new List<TagCommand>();

            foreach (var tag in request.Tags)
            {
                if (tags != null && !tags.Any(x => x.Name.ToLower() == tag.Name.ToLower()))
                {
                    tagsToCreate.Add(tag);
                }
            }

            await mediator.Send(new CreateEntitiesCommand<Tag, int>(tagsToCreate), cancellationToken);

            return Unit.Value;
        }
    }
}