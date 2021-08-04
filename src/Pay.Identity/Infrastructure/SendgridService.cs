using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;

using Pay.Identity.Configs;
using Pay.Identity.Domain.Emails;

namespace Pay.Identity.Infrastructure
{
    public class SendGridService : ISendEmailService
    {
        private SendgridConfiguration _sendgridConfig;
        private ReferenceUrls _referenceUrls;
        public SendGridService(
            IOptions<SendgridConfiguration> sendgridConfig,
            IOptions<ReferenceUrls> referenceUrls
        )
        {
            _sendgridConfig = sendgridConfig.Value;
            _referenceUrls = referenceUrls.Value;
        }
        public async Task SendEmailConfirmationEmail(
            string userId,
            string recipientEmail, 
            string recipientName, 
            string token
        )
        {
            var client = new SendGridClient(_sendgridConfig.ApiKey);
            var from = new EmailAddress(_sendgridConfig.SenderEmail, _sendgridConfig.SenderName);
            var subject = "Email Confirmation";
            var to = new EmailAddress(recipientEmail, recipientName);
            var url = $"{_referenceUrls.BaseUrl}/registration/confirmemail?userid={userId}&token={token}";
            var plainTextContent = $"Please confirm your email address: {url}";
            var htmlContent = $"Please confirm your email address <a href={url}>here</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}