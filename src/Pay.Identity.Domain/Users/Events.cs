using System;

namespace Pay.Identity.Domain.Users
{
    public static class Events {
        public static class V1
        {
            public record UserRegistered(string UserId, string Email, string EncryptedPassword, string FullName);
        }
    }
}