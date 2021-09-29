using System;

namespace Pay.Verification.Domain
{
    public static class Events {
        public static class V1
        {
            public record VerificationSubmitted(
                string CustomerId,
                string FirstName, 
                string LastName,
                string DateOfBirth,
                string Address1, 
                string Address2, 
                string CityTown, 
                string CountyState, 
                string Code, 
                string Country,
                string CountryCode,
                string ProofOfIdentity,
                string ProofOfAddress
            );

            public record VerificationCompleted(
                string CustomerId
            );

            public record CustomerCreated(
                string CustomerId);
            public record DateOfBirthAdded(
                string CustomerId, 
                string DateOfBirth);
            public record AddressAdded(
                string CustomerId, 
                string Address1, 
                string Address2, 
                string CityTown, 
                string CountyState, 
                string Code, 
                string Country,
                string CountryCode);
            public record CustomerDetailsSentForVerification(
                string CustomerId);

            public record CustomerDetailsVerified(
                string CustomerId,
                string CountryCode);
        }
    }
}