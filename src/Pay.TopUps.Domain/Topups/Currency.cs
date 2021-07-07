using System;

namespace Pay.TopUps.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }

    public record Currency
    {
        public string CurrencyCode { get; init; }
        public bool InUse { get; init; }
        public int DecimalPlaces { get; set; }

        public static Currency None = new Currency { InUse = false };
    }
}