namespace Newsweek.Handlers.Commands.News
{
    using System.Collections.Generic;

    using MediatR;

    using Newsweek.Data.Models;

    public class CreateNewsCommand : IRequest<IEnumerable<News>>
    {
        public CreateNewsCommand(IEnumerable<NewsCommand> news)
        {
            News = news;
        }

        public IEnumerable<NewsCommand> News { get; set; }
    }
}