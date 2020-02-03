namespace Newsweek.Handlers.Queries.Contracts
{
    using System.Threading.Tasks;

    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }

    public interface IQueryHandler<TResult>
    {
        Task<TResult> Handle();
    }
}