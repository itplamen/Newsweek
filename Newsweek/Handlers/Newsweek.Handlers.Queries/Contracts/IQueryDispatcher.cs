namespace Newsweek.Handlers.Queries.Contracts
{
    using System.Threading.Tasks;

    public interface IQueryDispatcher
    {
        Task<TResult> Dispatch<TResult>();

        Task<TResult> Dispatch<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>;
    }
}