using Microsoft.Extensions.Hosting;
using Newsweek.Worker.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Newsweek.Worker.Console
{
    public class TasksExecutor : IHostedService
    {
        private readonly INewsProvider newsProvider;

        public TasksExecutor(INewsProvider newsProvider)
        {
            this.newsProvider = newsProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await newsProvider.Get();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
