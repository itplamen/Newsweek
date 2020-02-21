namespace Newsweek.Handlers.Commands.Tags
{
    using System.Collections.Generic;
    
    using MediatR;
    
    public class CreateTagsCommand : IRequest
    {
        public CreateTagsCommand(IEnumerable<TagCommand> tags)
        {
            Tags = tags;
        }

        public IEnumerable<TagCommand> Tags { get; set; }
    }
}