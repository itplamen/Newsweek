namespace Newsweek.Handlers.Commands.News
{
    using System.Collections.Generic;

    using MediatR;

    public class CreateNewsCommand : IRequest<IEnumerable<Data.Models.News>>
    {
        public CreateNewsCommand(IEnumerable<NewsCommand> news)
        {
            News = news;
        }

        public IEnumerable<NewsCommand> News { get; set; }
    }
}