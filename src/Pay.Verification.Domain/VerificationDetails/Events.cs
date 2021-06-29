using System;

namespace Pay.Verification.Domain
{
    public static class Events {
        public static class V1
        {
            public record CustomerStartedVerification(string VerificationDetailsId, string CustomerId);
            public record DateOfBirthAdded(string VerificationDetailsId, string DateOfBirth);
            public record AddressAdded(string VerificationDetailsId, string Address1, string Address2, string CityTown, string CountyState, string Code, string Country);
            public record DetailsSubmitted(string VerificationDetailsId);
        }
    }
}