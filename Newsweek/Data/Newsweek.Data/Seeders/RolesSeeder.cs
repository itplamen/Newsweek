namespace Newsweek.Data.Seeders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newsweek.Common.Roles;
    using Newsweek.Data.Contracts;
    using Newsweek.Data.Models;

    public class RolesSeeder : ISeeder
    {
        public void Seed(NewsweekDbContext dbContext)
        {
            IEnumerable<string> roles = new List<string>()
            {
                ApplicationRoles.USER,
                ApplicationRoles.ADMINISTRATOR
            };

            foreach (var role in roles)
            {
                if (!dbContext.Roles.Any(x => x.Name == role))
                {
                    ApplicationRole applicationRole = new ApplicationRole()
                    {
                        Name = role,
                        CreatedOn = DateTime.UtcNow
                    };

                    dbContext.Roles.Add(applicationRole);
                }
            }
        }
    }
}