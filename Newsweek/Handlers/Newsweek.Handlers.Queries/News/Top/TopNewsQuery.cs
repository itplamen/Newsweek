﻿namespace Newsweek.Handlers.Queries.News.Top
{
    using System.Collections.Generic;

    using MediatR;

    public class TopNewsQuery<TViewModel> : IRequest<IEnumerable<TViewModel>>
        where TViewModel : class
    {
    }
}