namespace Newsweek.Worker.Core.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Newsweek.Handlers.Commands.News;

    public interface INewsProvider
    {
        Task<IEnumerable<NewsCommand>> Get();
    }
}