namespace Newsweek.Worker.Core.Providers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using MediatR;
    
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Commands.LogMessages;
    using Newsweek.Handlers.Commands.News;
    using Newsweek.Worker.Core.Contracts;
    
    public class NewsProviderLogging : INewsProvider
    {
        private readonly IMediator mediator;
        private readonly INewsProvider decorated;

        public NewsProviderLogging(IMediator mediator, INewsProvider decorated)
        {
            this.mediator = mediator;
            this.decorated = decorated;
        }

        public string Source => decorated.Source;

        public string Category => decorated.Category;

        public async Task<IEnumerable<NewsCommand>> Get(Source source, Category category)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            IEnumerable<NewsCommand> newsCommands = await decorated.Get(source, category);

            stopwatch.Stop();

            var message = new MessageCommand()
            {
                Action = $"{decorated.GetType().Name}",
                Request = $"{source?.Name}/{category?.Name}",
                Duration = stopwatch.Elapsed,
                Response = $"News: {newsCommands.Count()}"
            };

            var logMessageCommand = new LogMessageCommand(new List<MessageCommand>() { message });
            await mediator.Send(logMessageCommand);

            return newsCommands;
        }
    }
}