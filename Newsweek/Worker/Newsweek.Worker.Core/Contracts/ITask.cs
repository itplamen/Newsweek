namespace Newsweek.Worker.Core.Contracts
{
    using System.Threading.Tasks;

    public interface ITask
    {
        Task DoWork();
    }
}