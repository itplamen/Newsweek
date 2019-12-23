using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Newsweek.Worker.Core.Contracts
{
    public interface INewsProvider
    {
        Task Get();
    }
}
