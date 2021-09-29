using System;

namespace Pay.Verification
{
    public static class Commands
    {
        public static class V1
        {
            public class SubmitVerification
            {
                public string CustomerId { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string DateOfBirth { get; set; }
                public string Address1 { get; set; }
                public string Address2 { get; set; }
                public string CityTown { get; set; }
                public string CountyState { get; set; }
                public string Code { get; set; }
                public string Country { get; set; }
                public string CountryCode { get; set; }
                public string ProofOfIdentity { get; set; }
                public string ProofOfAddress { get; set; }
            }

            public class CompleteVerification
            {
                public string CustomerId { get; set; }
            }
        }
    }
}