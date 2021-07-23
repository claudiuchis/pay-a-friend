namespace Pay.Prepaid.Domain.PrepaidTransactions
{
    public record PrepaidTransactionId
    {
        public string TransactionId { get; init; }
        public PrepaidTransactionId(string id) => this.TransactionId = id;
        public static implicit operator string(PrepaidTransactionId self) => self.TransactionId;
    }
}
