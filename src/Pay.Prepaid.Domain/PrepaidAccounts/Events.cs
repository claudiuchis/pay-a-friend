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
                string CurrencyCode
            );
            public record PrepaidAccountDebited(
                string PrepaidAccountId,
                decimal Amount,
                string CurrencyCode
            );
        }
    }
}