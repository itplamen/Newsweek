namespace Newsweek.Data.Seeders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
                    Name = "Reuters",
                    Url = "https://www.reuters.com/"
                },
                new Source()
                {
                    Name = "talkSPORT",
                    Url = "https://talksport.com/"
                },
                new Source()
                {
                    Name = "ITworld",
                    Url = "https://www.itworld.com/"
                }
            };

            foreach (var source in sources)
            {
                if (!dbContext.Sources.Any(x => x.Name == source.Name))
                {
                    source.CreatedOn = DateTime.UtcNow;

                    dbContext.Sources.Add(source);
                }
            }
        }
    }
}