namespace Pay.TopUps.Domain
{
    public record PaymentResult(PaymentProvider PaymentProvider, string CardLast4Digits)
    {
        public string PaymentId { get; init; }
        public string Reason { get; init;}
    }
}