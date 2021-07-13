using System.Linq;
using System.Collections.Generic;
using Pay.Prepaid.Domain.Shared;

namespace Pay.Prepaid.Infrastructure
{
    public class FixedCurrencyLookup : ICurrencyLookup
    {
        private static readonly IEnumerable<Currency> _currencies =
            new[]
            {
                new Currency
                {
                    CurrencyCode = "EUR",
                    DecimalPlaces = 2,
                    InUse = true,
                    CountriesInUse = new string[] {
                        "IE", "FR", "IT", "ES", "DE", "PT", "NL", "GR", "AT", "FI", "SK", "MT", "LT"
                    }
                },
                new Currency
                {
                    CurrencyCode = "USD",
                    DecimalPlaces = 2,
                    InUse = true,
                    CountriesInUse = new string[] {
                        "US"
                    }
                },
                new Currency
                {
                    CurrencyCode = "GBP",
                    DecimalPlaces = 2,
                    InUse = true,
                    CountriesInUse = new string [] {
                        "GB"
                    }

                }
            };

        public Currency FindCurrency(string currencyCode)
        {
            var currency = _currencies.FirstOrDefault(x => x.CurrencyCode == currencyCode);
            return currency ?? Currency.None;
        }

        public Currency FindCurrencyByCountry(string countryCode)
        {
            var currency = _currencies.FirstOrDefault(x => x.CountriesInUse.Contains(countryCode));
            return currency ?? Currency.None;
        }

    }
}