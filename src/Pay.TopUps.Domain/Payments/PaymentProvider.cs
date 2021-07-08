namespace Pay.TopUps.Domain
{
    public record PaymentProvider
    {
        PaymentProvider(string value) => Value = value;
        public string Value { get; init; }
        public static PaymentProvider Stripe { get { return new PaymentProvider("Stripe"); }}
    }
}