using System.Collections.Generic;
using Eventuous.Projections.MongoDB.Tools;

namespace Pay.Prepaid.Projections
{
    public static class ReadModels
    {
        public record Transaction
        {
            public decimal Credit { get; init; }
            public decimal Debit { get; init; }
        }

        public record PrepaidAccount(
            string PrepaidAccountId,
            string CustomerId,
            string CurrencyCode,
            double Balance,
            IEnumerable<Transaction> Transactions
        ) : ProjectedDocument(PrepaidAccountId);

        public record TransferOrder(
            string TransferOrderId,
            string PayorPrepaidAccountId,
            string PayeePrepaidAccountId,
            decimal Amount,
            string CurrencyCode,
            string Status,
            string Reason,
            string Stage
        ) : ProjectedDocument(TransferOrderId);
    }
}