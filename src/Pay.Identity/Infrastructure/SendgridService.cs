using System;
using System.Net;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;
using FluentResults;

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
            if (string.IsNullOrWhiteSpace(sendgridConfig.Value.ApiKey))
                throw new ArgumentNullException(nameof(sendgridConfig.Value.ApiKey));

            if (string.IsNullOrWhiteSpace(sendgridConfig.Value.SenderEmail))
                throw new ArgumentNullException(nameof(sendgridConfig.Value.SenderEmail));

            if (string.IsNullOrWhiteSpace(sendgridConfig.Value.SenderName))
                throw new ArgumentNullException(nameof(sendgridConfig.Value.SenderName));

            if (string.IsNullOrWhiteSpace(referenceUrls.Value.BaseUrl))
                throw new ArgumentNullException(nameof(referenceUrls.Value.BaseUrl));

            _sendgridConfig = sendgridConfig.Value;
            _referenceUrls = referenceUrls.Value;
        }
        public async Task<Result> SendEmailConfirmationEmail(
            string userId,
            string recipientEmail, 
            string recipientName, 
            string token
        )
        {
            var result = Result.Merge(
                Result.FailIf(string.IsNullOrWhiteSpace(userId), new RequiredError(nameof(userId))),
                Result.FailIf(string.IsNullOrWhiteSpace(recipientEmail), new RequiredError(nameof(recipientEmail))),
                Result.FailIf(string.IsNullOrWhiteSpace(recipientName), new RequiredError(nameof(recipientName))),
                Result.FailIf(string.IsNullOrWhiteSpace(token), new RequiredError(nameof(token)))
            );

            if (result.IsFailed)
                return result;

            var client = new SendGridClient(_sendgridConfig.ApiKey);
            var from = new EmailAddress(_sendgridConfig.SenderEmail, _sendgridConfig.SenderName);
            var subject = "Email Confirmation";
            var to = new EmailAddress(recipientEmail, recipientName);
            var url = $"{_referenceUrls.BaseUrl}/registration/confirmemail?userid={userId}&token={token}";
            var plainTextContent = $"Please confirm your email address: {url}";
            var htmlContent = $"Please confirm your email address <a href={url}>here</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                return Result.Ok();
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Result.Fail(new UnauthorizedError());
                }
                return Result.Fail(new UnknownError());
            }
            
        }
    }
}