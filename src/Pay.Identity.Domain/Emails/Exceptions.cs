using System;

namespace Pay.Identity.Domain.Emails
{
    public class EmailConfirmationTokenInvalidException : Exception
    {
        public EmailConfirmationTokenInvalidException(string message): base(message)
        {}
    }
}