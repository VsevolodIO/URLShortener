using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using URLShortener.Areas.Identity.Services.Configurations;

namespace URLShortener.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<SendGridConfiguration> _config;


        public EmailSender(IOptions<SendGridConfiguration> config)
        {
            _config = config;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(_config.Value.SendGridApiKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_config.Value.SendGridFrom, _config.Value.SendGridSenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }

    }
}
