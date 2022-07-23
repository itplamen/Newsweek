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
                    Url = "https://www.euronews.com",
                    Description = @"Euronews, Europe’s number one news channel, empowers people to form their own opinion. 
                                    Euronews is unapologetically impartial and seeks to offer a diversity of viewpoints."
                },
                new Source()
                {
                    Name = "Al Jazeera",
                    Url = "https://www.aljazeera.com",
                    Description = @"News, analysis from the Middle East & worldwide, multimedia & interactives, opinions, 
                                    documentaries, podcasts, long reads and broadcast schedule."
                },
                new Source()
                {
                    Name = "talkSPORT",
                    Url = "https://talksport.com",
                    Description = @"Tune in to the world's biggest sports radio station - Live Premier League football coverage, 
                                    breaking sports news, transfer rumours & exclusive interviews."
                },
                new Source()
                {
                    Name = "CIO",
                    Url = "https://www.cio.com",
                    Description = @"Find the peer insights and expert advice IT leaders need to create business value with 
                                    technology, drive innovation and develop their careers."
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