namespace Newsweek.Data.Seeders
{
    using System;
    using System.Collections.Generic;

    using Newsweek.Data.Contracts;
    using Newsweek.Data.Models;

    public class SourcesSeeder : ISeeder
    {
        public void Seed(NewsweekDbContext dbContext)
        {
            IEnumerable<Source> sources = new List<Source>()
            {
                new Source()
                {
                    Name = "Euronews",
                    Url = "https://www.euronews.com/"
                },
                new Source()
                {
                    Name = "Sky Sports",
                    Url = "https://www.skysports.com/"
                }
            };

            foreach (var source in sources)
            {
                source.CreatedOn = DateTime.UtcNow;

                dbContext.Add(source);
            }
        }
    }
}