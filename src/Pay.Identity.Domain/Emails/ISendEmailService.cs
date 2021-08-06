using System.Threading.Tasks;
using FluentResults;

namespace Pay.Identity.Domain.Emails
{
    public interface ISendEmailService
    {
        Task<Result> SendEmailConfirmationEmail(
            string userId,
            string recipientEmail, 
            string recipientName, 
            string token
        );
    }
}