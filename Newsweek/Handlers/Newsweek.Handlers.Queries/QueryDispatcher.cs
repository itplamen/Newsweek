namespace Newsweek.Handlers.Queries
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using Newsweek.Handlers.Queries.Contracts;

    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TResult Dispatch<TResult>()
        {
            IQueryHandler<TResult> handler = serviceProvider.GetRequiredService<IQueryHandler<TResult>>();

            return handler.Handle();
        }

        public TResult Dispatch<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            IQueryHandler<TQuery, TResult> handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

            return handler.Handle(query);
        }
    }
}