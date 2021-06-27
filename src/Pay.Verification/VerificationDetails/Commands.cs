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
        }
    }
}