namespace Newsweek.Handlers.Commands.LogMessages
{
    using System;

    using AutoMapper;

    using MediatR;
    
    using Newsweek.Common.Infrastructure.Mapping;
    using Newsweek.Data.Models;
    
    public class MessageCommand : IRequest, IMapTo<LogMessage>, IHaveCustomMappings
    {
        public string Action { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public TimeSpan Duration { get; set; }

        public bool LogToFile { get; set; }

        public bool HasErrors { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<MessageCommand, LogMessage>()
                .ForMember(x => x.Type, opt => opt.MapFrom(x => 
                    x.HasErrors ? LogMessageType.Error : LogMessageType.Information));
        }
    }
}