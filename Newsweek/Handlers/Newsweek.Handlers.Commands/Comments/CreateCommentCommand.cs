namespace Newsweek.Handlers.Commands.Comments
{
    using MediatR;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class CreateCommentCommand : IRequest, IMapTo<Comment>
    {
        public string Content { get; set; }

        public string UserId { get; set; }

        public int NewsId { get; set; }
    }
}