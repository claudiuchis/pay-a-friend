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
                public string Name { get; set; }
                public string Number { get; set; }
                public string ExpMonth { get; set; }
                public string ExpYear { get; set; }
                public string Cvc { get; set; }
                public string AddressCity { get; set; }
                public string AddressCountry { get; set; }
                public string AddressLine1 { get; set; }
                public string AddressLine2 { get; set; }
                public string AddressState { get; set; }
                public string AddressZip { get; set; }
            }
        }
    }
}