using Eventuous.Projections.MongoDB.Tools;

namespace Pay.Verification.Projections
{
    public static class ReadModels
    {
        public record VerificationDetails(
            string DetailsId,
            string CustomerId
        ): ProjectedDocument(DetailsId);
    }
}