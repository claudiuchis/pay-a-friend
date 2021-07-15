using System;

namespace Pay.Prepaid.Domain.PrepaidAccounts
{
    public record PrepaidTransactionType {
        public string Value;
        PrepaidTransactionType(string value) => Value = value;
        public static PrepaidTransactionType TopUp { get { return new PrepaidTransactionType("TopUp"); }}
        public static PrepaidTransactionType Transfer { get { return new PrepaidTransactionType("Transfer"); }}
        public static PrepaidTransactionType Refund { get { return new PrepaidTransactionType("Refund"); }}
        public static implicit operator string(PrepaidTransactionType self) => self.Value;
    }
}