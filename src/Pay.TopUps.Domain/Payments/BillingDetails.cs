
namespace Pay.TopUps.Domain
{
    public record BillingDetails()
    {
        public string AddressCity { get; init; }
        public string AddressCountry { get; init; }
        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string AddressState { get; init; }
        public string AddressZip { get; init; }

        BillingDetails() {}
        
        public BillingDetails(
            string addressCity,
            string addressCountry,
            string addressLine1,
            string addressLine2,
            string addressState,
            string addressZip)
        {
            AddressCity = addressCity;
            AddressCountry = addressCountry;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            AddressState = addressState;
            AddressZip = addressZip;
        }

    }
}