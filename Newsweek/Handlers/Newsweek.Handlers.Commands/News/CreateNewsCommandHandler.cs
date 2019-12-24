namespace Newsweek.Handlers.Commands.News
{
    using System;

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

        public void Handle(CreateNewsCommand command)
        {
            NewsData news = Mapper.Map<NewsData>(command);
            news.CreatedOn = DateTime.UtcNow;

            dbContext.Add(news);
            dbContext.SaveChanges();
        }
    }
}