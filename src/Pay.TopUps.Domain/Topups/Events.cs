
namespace Pay.TopUps.Domain
{
    public static class Events
    {
        public static class V1
        {
            public record TopUpCompleted(string TopUpId, decimal Amount, string CurrencyCode);
            public record TopUpFailed(string TopUpId, decimal Amount, string CurrencyCode);

        }
    }
}