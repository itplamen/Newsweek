namespace Newsweek.Handlers.Commands.NewsTags
{
    using System.Collections.Generic;

    using MediatR;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.News;

    public class CreateNewsTagsCommand : IRequest
    {
        public CreateNewsTagsCommand(IEnumerable<NewsCommand> newsCommands, IEnumerable<News> news, IEnumerable<Tag> tags)
        {
            NewsCommands = newsCommands;
            News = news;
            Tags = tags;
        }

        public IEnumerable<NewsCommand> NewsCommands { get; set; }

        public IEnumerable<News> News { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}