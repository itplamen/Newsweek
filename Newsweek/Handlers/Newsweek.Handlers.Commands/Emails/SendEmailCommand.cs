namespace Newsweek.Handlers.Commands.Emails
{
    using System.Collections.Generic;

    using MediatR;

    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;

    public class SendEmailCommand : IRequest, IMapTo<Email>
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public IEnumerable<string> Emails { get; set; }
    }
}
