namespace Pay.Prepaid.Domain.PrepaidTransactions
{
    public record PrepaidTransaction
    {
        public PrepaidTransactionType TransactionType { get; init; }
        public PrepaidTransactionId TransactionId { get; init; }
        public PrepaidTransaction(PrepaidTransactionId id, PrepaidTransactionType type)
        {
            TransactionType = type;
            TransactionId = id;
        }
    }
}