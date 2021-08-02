using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

using Pay.Identity.Configs;
using Pay.Identity.Domain.Emails;

namespace Pay.Identity.Infrastructure
{
    public class SendgridService : ISendEmailService
    {
        private SendgridConfiguration _sendgridConfig;
        public SendgridService(
            IOptions<SendgridConfiguration> sendgridConfig
        )
        {
            _sendgridConfig = sendgridConfig.Value;
        }
        public async Task SendEmailConfirmationEmail(string recipientEmail, string recipientName, string token)
        {
            var client = new SendGridClient(_sendgridConfig.ApiKey);
            var from = new EmailAddress(_sendgridConfig.SenderEmail, "The Sender");
            var subject = "Test Email with SendGrid";
            var to = new EmailAddress(recipientEmail, recipientName);
            var plainTextContent = "Test Email with SendGrid C# Library";
            var htmlContent = "<strong>HTML text for the Test Email</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}