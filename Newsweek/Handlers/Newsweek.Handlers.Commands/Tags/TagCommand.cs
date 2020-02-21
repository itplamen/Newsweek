namespace Newsweek.Handlers.Commands.Tags
{
    using MediatR;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    
    public class TagCommand : IRequest, IMapTo<Tag>
    {
        public TagCommand(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}