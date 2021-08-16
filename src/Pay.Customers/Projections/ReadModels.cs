using Eventuous.Projections.MongoDB.Tools;

namespace Pay.Verification.Projections
{
    public static class ReadModels
    {
        public record CustomerDetails(
            string CustomerId, 
            bool DetailsSubmitted,
            bool DetailsVerified 
        ) 
            : ProjectedDocument(CustomerId)
        {
            public string DateOfBirth { get; init; }
            public string Address { get; init; }
        }
    }
}