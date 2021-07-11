namespace Pay.Prepaid.Reactors
{
    public static class IntegrationEvents
    {
        public static class v1
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