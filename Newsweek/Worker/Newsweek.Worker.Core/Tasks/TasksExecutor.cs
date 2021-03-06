﻿namespace Newsweek.Worker.Core.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Hosting;

    using Newsweek.Handlers.Commands.LogMessages;
    using Newsweek.Worker.Core.Contracts;

    public class TasksExecutor : BackgroundService
    {
        private const int FIVE_MINUTES_DELAY = 1000 * 60 * 5;

        private readonly ITask newsTask;
        private readonly IMediator mediator;

        public TasksExecutor(ITask newsTask, IMediator mediator)
        {
            this.newsTask = newsTask;
            this.mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = new MessageCommand();
                message.Action = $"{nameof(TasksExecutor)} -> {nameof(ExecuteAsync)}";

                Stopwatch stopwatch = Stopwatch.StartNew();

                try
                {
                    await newsTask.DoWork();
                }
                catch (Exception ex)
                {
                    message.HasErrors = true;
                    message.Response = $"{ex.Message} - {ex.StackTrace}";
                }
                finally
                {
                    message.Duration = stopwatch.Elapsed;

                    var request = new LogMessageCommand(new List<MessageCommand>() { message });
                    await mediator.Send(request, stoppingToken);

                    await Task.Delay(FIVE_MINUTES_DELAY, stoppingToken);
                }
            }
        }
    }
}