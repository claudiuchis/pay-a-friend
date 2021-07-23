namespace Pay.Prepaid.Domain.PrepaidAccounts
{
    public static class Events
    {
        public static class V1
        {
            public record PrepaidAccountCreated(
                string PrepaidAccountId,
                string CustomerId,
                string CurrencyCode
            );
            public record PrepaidAccountCredited(
                string PrepaidAccountId,
                decimal Amount,
                string CurrencyCode,
                string TransactionType,
                string TransactionId
            );
            public record PrepaidAccountDebited(
                string PrepaidAccountId,
                decimal Amount,
                string CurrencyCode,
                string TransactionType,
                string TransactionId
            );
            public record PrepaidAccountHoldPlaced(
                string PrepaidAccountId,
                decimal Amount,
                string CurrencyCode,
                string TransactionType,
                string TransactionId
            );
            public record PrepaidAccountHoldReleased(
                string PrepaidAccountId,
                string TransactionId,
                decimal Amount,
                string CurrencyCode,
                string Reason
            );


        }
    }
}