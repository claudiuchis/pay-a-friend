using System;

namespace Pay.TopUps.Domain
{
    public record TopUpAmount : Money 
    {
        TopUpAmount(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) 
            : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 0)
                throw new ArgumentException(
                    $"amount cannot be negative",
                    nameof(amount)
                );
        }

        internal TopUpAmount(decimal amount, string currencyCode)
            : base(amount, new Currency { CurrencyCode = currencyCode}) {}

        public new static TopUpAmount FromDecimal(
            decimal amount,
            string currencyCode,
            ICurrencyLookup currencyLookup) 
            => new TopUpAmount(amount, currencyCode, currencyLookup);

    }
}