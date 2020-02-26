namespace Newsweek.Handlers.Commands.Emails
{
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
	using System.Threading.Tasks;

	using MediatR;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    
    using SendGrid;
	using SendGrid.Helpers.Mail;

	public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
    {
		private readonly ISendGridClient client;
        private readonly ILogger<SendEmailCommandHandler> logger;
        private readonly IConfiguration configuration;

        public SendEmailCommandHandler(ISendGridClient client, ILogger<SendEmailCommandHandler> logger, IConfiguration configuration)
		{
			this.client = client;
            this.logger = logger;
            this.configuration = configuration;
		}

		public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                EmailAddress from = new EmailAddress(configuration["SendGrid:FromEmail"]);
                List<EmailAddress> to = request.Emails
                    .Select(x => new EmailAddress(x))
                    .ToList();

                SendGridMessage message = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, request.Subject, null, request.Content);

                Response response = await client.SendEmailAsync(message, cancellationToken);
                logger.LogInformation("Sent email successfully", request, response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not send email", request);
            }

            return Unit.Value;
		}
    }
}