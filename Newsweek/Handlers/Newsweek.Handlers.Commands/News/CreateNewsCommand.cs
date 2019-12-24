namespace Newsweek.Handlers.Commands.News
{
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Handlers.Commands.Contracts;
    using NewsData = Newsweek.Data.Models.News;

    public class CreateNewsCommand : ICommand, IMapTo<NewsData>
    {
        public CreateNewsCommand(
            string title, 
            string description, 
            string content, 
            string remoteUrl, 
            string mainImageUrl, 
            int sourceId)
        {
            Title = title;
            Description = description;
            Content = content;
            RemoteUrl = remoteUrl;
            MainImageUrl = mainImageUrl;
            SourceId = sourceId;
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string RemoteUrl { get; set; }

        public string MainImageUrl { get; set; }

        public int SourceId { get; set; }
    }
}