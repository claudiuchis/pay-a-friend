using System;

namespace Pay.Prepaid.Domain.PrepaidTransactions
{
    public record PrepaidTransactionType {
        public string Value;
        PrepaidTransactionType(string value) => Value = value;
        public static PrepaidTransactionType TopUp { get { return new PrepaidTransactionType("TopUp"); }}
        public static PrepaidTransactionType TransferBetweenAccounts { get { return new PrepaidTransactionType("TransferBetweenAccounts"); }}
        public static PrepaidTransactionType Refund { get { return new PrepaidTransactionType("Refund"); }}
        public static implicit operator string(PrepaidTransactionType self) => self.Value;
        public static PrepaidTransactionType FromString(string value) {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("transaction type cannot be null");

            if (!value.Equals(PrepaidTransactionType.TopUp) && 
                !value.Equals(PrepaidTransactionType.TransferBetweenAccounts) &&
                !value.Equals(PrepaidTransactionType.TransferBetweenAccounts))
                throw new ArgumentException($"{value} not supported as a transaction type");
            return new PrepaidTransactionType(value);
        }
    }
}