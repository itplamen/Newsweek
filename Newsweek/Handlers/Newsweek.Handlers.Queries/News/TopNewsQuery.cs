namespace Newsweek.Handlers.Queries.News
{
    using System.Collections.Generic;

    using MediatR;

    public class TopNewsQuery<TViewModel> : IRequest<IEnumerable<TViewModel>>
        where TViewModel : class
    {
        public TopNewsQuery(int take)
        {
            Take = take;
        }

        public int Take { get; set; }
    }
}