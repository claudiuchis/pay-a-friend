using System;

namespace Pay.Identity.Domain.Emails
{
    public record EmailConfirmationToken
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
        public EmailConfirmationToken(string token, DateTime validTo)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            Token = token;
            ValidTo = validTo;
        }

        public bool IsTokenValid(string token)
        {
            if (token!= null && Token.Equals(token) && DateTime.Now <= ValidTo )
                return true;

            return false;
        }
    }
}