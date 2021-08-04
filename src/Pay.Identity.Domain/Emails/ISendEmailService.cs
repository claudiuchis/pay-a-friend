using System.Threading.Tasks;

namespace Pay.Identity.Domain.Emails
{
    public interface ISendEmailService
    {
        Task SendEmailConfirmationEmail(
            string userId,
            string recipientEmail, 
            string recipientName, 
            string token
        );
    }
}