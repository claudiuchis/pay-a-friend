using System;

namespace Pay.Verification.Domain
{
    public static class Events {
        public static class V1
        {
            public record CustomerStartedVerification(string VerificationDetailsId, string CustomerId);
        }
    }
}