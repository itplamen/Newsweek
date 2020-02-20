namespace Newsweek.Handlers.Commands.Tags
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.Contracts;

    public class TagCommand : ICommand, IMapTo<Tag>
    {
        public TagCommand(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}