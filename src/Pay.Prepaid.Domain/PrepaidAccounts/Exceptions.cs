using System;

namespace Pay.Prepaid.Domain.PrepaidAccounts
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message) : base(message)
        {
        }
    }

}