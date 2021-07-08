
namespace Pay.TopUps.Domain
{
    public static class Events
    {
        public static class V1
        {
            public record TopUpCompleted(
                string TopUpId, 
                decimal Amount, 
                string CurrencyCode, 
                string PaymentProvider,
                string PaymentId,
                string CardLast4Digits);
            public record TopUpFailed(
                string TopUpId, 
                decimal Amount, 
                string CurrencyCode,
                string PaymentProvider,
                string Reason,
                string CardLast4Digits);
        }
    }
}