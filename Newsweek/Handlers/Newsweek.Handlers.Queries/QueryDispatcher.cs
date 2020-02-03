namespace Newsweek.Handlers.Queries
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    using Newsweek.Handlers.Queries.Contracts;

    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<TResult> Dispatch<TResult>()
        {
            IQueryHandler<TResult> handler = serviceProvider.GetRequiredService<IQueryHandler<TResult>>();

            return await handler.Handle();
        }

        public async Task<TResult> Dispatch<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            IQueryHandler<TQuery, TResult> handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

            return await handler.Handle(query);
        }
    }
}