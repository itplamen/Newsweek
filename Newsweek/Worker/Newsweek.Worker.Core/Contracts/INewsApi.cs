namespace Newsweek.Worker.Core.Contracts
{
    using System.Threading.Tasks;

    using AngleSharp.Dom;

    public interface INewsApi
    {
        Task<IDocument> Get(string url);
    }
}