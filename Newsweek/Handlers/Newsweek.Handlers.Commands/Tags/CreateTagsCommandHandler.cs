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

    public class CreateTagsCommandHandler : IRequestHandler<CreateTagsCommand>
    {
        private readonly IMediator mediator;

        public CreateTagsCommandHandler(Mediator mediator)
        {
            this.mediator = mediator;
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

            IEnumerable<Tag> tags = await mediator.Send(query, cancellationToken);

            ICollection<TagCommand> tagsToCreate = new List<TagCommand>();

            foreach (var tag in request.Tags)
            {
                if (tags != null && !tags.Any(x => x.Name.ToLower() == tag.Name.ToLower()))
                {
                    tagsToCreate.Add(tag);
                }
            }

            await mediator.Send(new CreateEntitiesCommand<Tag>(tagsToCreate), cancellationToken);

            return Unit.Value;
        }
    }
}