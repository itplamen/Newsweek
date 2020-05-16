namespace Newsweek.Worker.Core.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.News;

    public interface INewsProvider
    {
        string Source { get; }

        string Category { get; }

        Task<IEnumerable<NewsCommand>> Get(Source source, Category category);
    }
}