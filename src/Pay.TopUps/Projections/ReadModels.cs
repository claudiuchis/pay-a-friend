using Eventuous.Projections.MongoDB.Tools;

namespace Pay.TopUps.Projections
{
    public static class ReadModels
    {
        public record TopUpDetails(
            string TransactionId, 
            decimal Amount, 
            string CurrencyCode, 
            string CustomerId, 
            string PaymentMethod, 
            string CardLast4Digits
        ) : ProjectedDocument(TransactionId);
    }
}