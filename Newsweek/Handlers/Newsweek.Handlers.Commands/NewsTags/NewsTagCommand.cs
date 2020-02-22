namespace Newsweek.Handlers.Commands.NewsTags
{
    using MediatR;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class NewsTagCommand : IRequest, IMapTo<NewsTag>
    {
        public NewsTagCommand(int newsId, int tagId)
        {
            NewsId = newsId;
            TagId = tagId;
        }

        public int NewsId { get; set; }

        public int TagId { get; set; }
    }
}