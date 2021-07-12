using System;

namespace Pay.Prepaid.Domain.PrepaidAccounts
{
    public record PrepaidTransactionType {
        public string Value;
        PrepaidTransactionType(string value) => Value = value;
        public static PrepaidTransactionType TopUp { get { return new PrepaidTransactionType("TopUp"); }}
        public static implicit operator string(PrepaidTransactionType self) => self.Value;
    }
}