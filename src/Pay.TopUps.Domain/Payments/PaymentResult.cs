namespace Pay.TopUps.Domain
{
    public record PaymentResult
    {
        public PaymentProvider PaymentProvider { get; init; }
        public string PaymentId { get; init; }
        public string CardLast4Digits { get; init; }
        public string Reason { get; init; }
    }
}