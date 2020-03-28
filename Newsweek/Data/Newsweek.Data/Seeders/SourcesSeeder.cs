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
                    Name = "Reuters",
                    Url = "https://www.reuters.com",
                    Description = @"Reuters.com brings you the latest news from around the world, covering breaking news in markets, 
                                    business, politics, entertainment, technology, video and pictures."
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
                    Name = "ITworld",
                    Url = "https://www.itworld.com",
                    Description = @"ITworld offers technology decision makers, business leaders and other IT influencers a unique 
                                    environment for gathering and sharing information that will help them do their jobs with efficiency and authority"
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