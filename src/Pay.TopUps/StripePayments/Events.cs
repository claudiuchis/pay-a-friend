namespace Pay.TopUps.StripePayments
{
    public static class Events
    {
        public static class V1
        {
            public record TopUpCompleted(
                string TransactionId, 
                decimal Amount, 
                string CurrencyCode, 
                string CustomerId,
                string PaymentMethod,
                string CardLast4Digits);
        }
    }
}