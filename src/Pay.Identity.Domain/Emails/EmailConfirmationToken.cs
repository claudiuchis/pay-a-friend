using System;

namespace Pay.Identity.Domain.Emails
{
    public record EmailConfirmationToken(string Token, DateTime ValidTo);
}