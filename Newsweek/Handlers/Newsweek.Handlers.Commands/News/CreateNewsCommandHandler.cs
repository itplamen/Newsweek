namespace Newsweek.Handlers.Commands.News
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;

    using Newsweek.Data;
    using Newsweek.Handlers.Commands.Contracts;
    using NewsData = Newsweek.Data.Models.News;

    public class CreateNewsCommandHandler : ICommandHandler<CreateNewsCommand>
    {
        private readonly NewsweekDbContext dbContext;

        public CreateNewsCommandHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(CreateNewsCommand command)
        {
            NewsData news = Mapper.Map<NewsData>(command);
            news.CreatedOn = DateTime.UtcNow;

            await dbContext.AddAsync(news);
            await dbContext.SaveChangesAsync();
        }
    }
}