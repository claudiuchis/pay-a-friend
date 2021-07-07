namespace Pay.TopUps.Commands
{
    public static class Commands
    {
        public static class V1
        {
            public class SubmitTopUp
            {
                public string TopUpId { get; set; }
                public decimal Amount { get; set; }
                public string CurrencyCode { get; set; }
            }
        }
    }
}