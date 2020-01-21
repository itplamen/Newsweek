namespace Newsweek.Handlers.Commands.News
{
    using System.Collections.Generic;

    using Newsweek.Handlers.Commands.Contracts;

    public class CreateNewsCommand : ICommand
    {
        public CreateNewsCommand(IEnumerable<NewsCommand> news)
        {
            News = news;
        }

        public IEnumerable<NewsCommand> News { get; set; }
    }
}