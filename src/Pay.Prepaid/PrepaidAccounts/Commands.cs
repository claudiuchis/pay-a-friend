namespace Pay.Prepaid.PrepaidAccounts
{
    public static class Commands
    {
        public static class V1
        {
            public record CreatePrepaidAccount(
                string PrepaidAccountId, 
                string CustomerId, 
                string CountryCode);
            public record CreditPrepaidAccount(
                string PrepaidAccountId, 
                decimal Amount, 
                string CurrencyCode,
                string TransactionType,
                string TransactionId);
            public record PlaceHoldOnPrepaidAccount(
                string PrepaidAccountId, 
                decimal Amount, 
                string CurrencyCode,
                string TransactionType,
                string TransactionId);
            public record ReleaseHoldOnPrepaidAccount(
                string PrepaidAccountId, 
                string TransactionId,
                string Reason);

            public record DebitPrepaidAccount(
                string PrepaidAccountId, 
                decimal Amount, 
                string CurrencyCode,
                string TransactionType,
                string TransactionId);
        }
    }
}