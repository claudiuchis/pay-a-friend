using System;

namespace Pay.Identity.Queries
{
    public static class ReadModels
    {
        public record UserDetails (string UserId, string Email, string HashedPassword, string FullName);
    }
}