using System;

namespace Pay.Prepaid.Domain
{
    public record Funds : Money
    {
        Funds(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
            : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 0)
                throw new ArgumentException(
                    "Funds cannot be negative",
                    nameof(amount)
                );
        }

        internal Funds(decimal amount, string currencyCode)
            : base(amount, new Currency {CurrencyCode = currencyCode}) { }

        public new static Funds FromDecimal(
            decimal amount,
            string currency,
            ICurrencyLookup currencyLookup)
            => new Funds(amount, currency, currencyLookup);
    }
}