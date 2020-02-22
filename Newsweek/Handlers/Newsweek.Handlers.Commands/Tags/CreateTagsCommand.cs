namespace Newsweek.Handlers.Commands.Tags
{
    using System.Collections.Generic;
    
    using MediatR;

    using Newsweek.Data.Models;

    public class CreateTagsCommand : IRequest<IEnumerable<Tag>>
    {
        public CreateTagsCommand(IEnumerable<TagCommand> tags)
        {
            Tags = tags;
        }

        public IEnumerable<TagCommand> Tags { get; set; }
    }
}