namespace Newsweek.Handlers.Commands.Tags
{
    using System.Collections.Generic;

    using Newsweek.Handlers.Commands.Contracts;

    public class CreateTagsCommand : ICommand
    {
        public CreateTagsCommand(IEnumerable<TagCommand> tags)
        {
            Tags = tags;
        }

        public IEnumerable<TagCommand> Tags { get; set; }
    }
}