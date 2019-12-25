namespace Newsweek.Worker.Core.Tasks
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;
    
    using Newsweek.Worker.Core.Contracts;

    public class TasksExecutor : IHostedService
    {
        private readonly ITask newsTask;

        public TasksExecutor(ITask newsTask)
        {
            this.newsTask = newsTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await newsTask.DoWork();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}