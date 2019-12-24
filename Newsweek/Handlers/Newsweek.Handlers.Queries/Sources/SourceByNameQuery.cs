namespace Newsweek.Handlers.Queries.Sources
{
    using Newsweek.Data.Models;
    using Newsweek.Handlers.Queries.Contracts;

    public class SourceByNameQuery : IQuery<Source>
    {
        public SourceByNameQuery(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}