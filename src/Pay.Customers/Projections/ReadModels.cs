using Eventuous.Projections.MongoDB.Tools;

namespace Pay.Verification.Projections
{
    public static class ReadModels
    {
        public record Customer(
            string CustomerId, 
            string VerificationStatus) 
            : ProjectedDocument(CustomerId)
        {
            public string DateOfBirth { get; init; }
            public string Address { get; init; }
        }
    }

    public class VerificationStatus
    {
        private VerificationStatus(string value) => Value = value;
        public string Value { get; private set; }
        public static VerificationStatus DetailsNotVerified { get { return new VerificationStatus("DetailsNotVerified"); }}
        public static VerificationStatus DetailsSentForVerification { get { return new VerificationStatus("DetailsSentForVerification"); }}
        public static VerificationStatus DetailsPartiallyVerified { get { return new VerificationStatus("DetailsPartiallyVerified"); }}
        public static VerificationStatus DetailsVerified { get { return new VerificationStatus("DetailsVerified"); }}

        public static implicit operator string(VerificationStatus self) => self.Value;
    }
}