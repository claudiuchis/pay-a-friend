namespace Pay.Prepaid.TopUps
{
    public static class Commands
    {
        public static class V1
        {
            public record CreatePrepaidAccount(string PrepaidAccountId, string CustomerId, string CurrencyCode);
            public record CreditPrepaidAccount(string PrepaidAccountId, decimal Amount, string CurrencyCode);
            public record DebitPrepaidAccount(string PrepaidAccountId, decimal Amount, string CurrencyCode);
        }
    }
}