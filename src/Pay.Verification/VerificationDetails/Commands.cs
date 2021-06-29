using System;

namespace Pay.Verification
{
    public static class Commands
    {
        public static class V1
        {
            public class CreateVerificationDetailsDraft
            {
                public string VerificationDetailsId { get; set; }
                public string CustomerId { get; set; }
            }

            public class AddDateOfBirth
            {
                public string VerificationDetailsId { get; set; }
                public string DateOfBirth { get; set; }
            }

            public class AddAddress
            {
                public string VerificationDetailsId { get; set; }
                public string Address1 { get; set; }
                public string Address2 { get; set; }
                public string CityTown { get; set; }
                public string CountyState { get; set; }
                public string Code { get; set; }
                public string Country { get; set; }
            }

            public class SubmitDetails
            {
                public string VerificationDetailsId { get; set; }
            }
            
        }
    }
}