using System;
using System.Collections.Generic;
using System.Text;

namespace Newsweek.Data.Contracts
{
    public interface ISeeder
    {
        void Seed(NewsweekDbContext dbContext);
    }
}
