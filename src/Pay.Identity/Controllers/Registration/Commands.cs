using System;

namespace Pay.Identity.Registration
{
    public static class Commands
    {
        public static class V1
        {
            public record RegisterUser(
                string UserId,
                string Email, 
                string Password,
                string FullName);

            public record SendConfirmationEmail(
                string UserId);

            public record ConfirmEmail(
                string UserId,
                string Token
            );
        }
    }
}