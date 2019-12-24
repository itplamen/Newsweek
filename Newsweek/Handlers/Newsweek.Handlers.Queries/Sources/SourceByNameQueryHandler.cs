namespace Newsweek.Handlers.Queries.Sources
{
    using System.Linq;

    using Newsweek.Data;
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Contracts;

    public class SourceByNameQueryHandler : IQueryHandler<SourceByNameQuery, Source>
    {
        private readonly NewsweekDbContext dbContext;

        public SourceByNameQueryHandler(NewsweekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Source Handle(SourceByNameQuery query)
        {
            IQueryable<Source> sources = dbContext.Sources;
            Source source = sources.Where(x => x.Name == query.Name).FirstOrDefault();

            return source;
        }
    }
}