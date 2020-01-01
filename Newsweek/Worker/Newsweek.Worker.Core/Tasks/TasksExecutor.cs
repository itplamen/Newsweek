namespace Newsweek.Worker.Core.Tasks
{
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;
    
    using Newsweek.Worker.Core.Contracts;

    public class TasksExecutor : BackgroundService
    {
        private const int FIVE_MINUTES_DELAY = 1000 * 60 * 5;

        private readonly ITask newsTask;

        public TasksExecutor(ITask newsTask)
        {
            this.newsTask = newsTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await newsTask.DoWork();

                await Task.Delay(FIVE_MINUTES_DELAY, stoppingToken);
            }
        }
    }
}