using System;
using Eventuous.Projections.MongoDB.Tools;

namespace Pay.Identity.Projections
{
    public static class ReadModels
    {
        public record UserDetails (string UserId, string Email, string HashedPassword, string FullName): ProjectedDocument(UserId);
    }
}