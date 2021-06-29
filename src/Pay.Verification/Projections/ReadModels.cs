using Eventuous.Projections.MongoDB.Tools;

namespace Pay.Verification.Projections
{
    public static class ReadModels
    {
        public record VerificationDetails(string DetailsId, string CustomerId, string Status) : ProjectedDocument(DetailsId)
        {
            public string DateOfBirth { get; init; }
            public string Address { get; init; }
        }
    }

    public class VerificationStatus
    {
        private VerificationStatus(string value) => Value = value;
        public string Value { get; private set; }
        public static VerificationStatus Draft { get { return new VerificationStatus("Draft"); }}
        public static VerificationStatus Pending { get { return new VerificationStatus("Pending"); }}
        public static VerificationStatus Approved { get { return new VerificationStatus("Approved"); }}
    }
}