using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Pay.Identity.Infrastructure;
using Pay.Identity.Configs;

namespace Pay.Identity.Test
{
    public class SendGridServiceTests
    {

        [Fact]
        public void NoApiKey()
        {
            var sendGridConfig = new SendgridConfiguration() {
                ApiKey = "",
                SenderEmail = "",
                SenderName = ""
            };
            var referenceUrls = new ReferenceUrls() {
                BaseUrl = ""
            };

            var sendGridConfigOptions = Options.Create(sendGridConfig);
            var referenceUrlsOptions = Options.Create(referenceUrls);

            Assert.Throws<ArgumentNullException>(() => new SendGridService(sendGridConfigOptions, referenceUrlsOptions));
        }

        [Fact]
        public async Task InvalidApiKey()
        {
            var sendGridConfig = new SendgridConfiguration() {
                ApiKey = "123",
                SenderEmail = "fromme@me.com",
                SenderName = "me"
            };
            var referenceUrls = new ReferenceUrls() {
                BaseUrl = "localhost"
            };

            var sendGridConfigOptions = Options.Create(sendGridConfig);
            var referenceUrlsOptions = Options.Create(referenceUrls);

            var service = new SendGridService(sendGridConfigOptions, referenceUrlsOptions);

            var userId = "123";
            var recipientEmail = "tome@me.com";
            var recipientName = "Me";
            var token = "456";

            var result = await service.SendEmailConfirmationEmail(
                userId,
                recipientEmail,
                recipientName,
                token
            );

            result.IsFailed.Equals(true);
        }

        [Fact]
        public async Task SendEmailSuccessfully()
        {
            var sendGridConfig = new SendgridConfiguration() {
                ApiKey = "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",
                SenderEmail = "from@me.com",
                SenderName = "me"
            };
            var referenceUrls = new ReferenceUrls() {
                BaseUrl = "localhost"
            };

            var sendGridConfigOptions = Options.Create(sendGridConfig);
            var referenceUrlsOptions = Options.Create(referenceUrls);

            var service = new SendGridService(sendGridConfigOptions, referenceUrlsOptions);

            var userId = "123";
            var recipientEmail = "to@me.com";
            var recipientName = "Me";
            var token = "456";

            var result = await service.SendEmailConfirmationEmail(
                userId,
                recipientEmail,
                recipientName,
                token
            );

            result.IsSuccess.Equals(true);
        }

    }
}
