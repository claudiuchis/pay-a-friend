using System;

namespace Pay.TopUps.Domain
{
    public record Money
    {
        public decimal Amount { get; init; }
        public Currency Currency { get; init; }
        protected Money(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) {
            if (String.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException(
                    nameof(currencyCode), "Currency code must be specified");

            var currency = currencyLookup.FindCurrency(currencyCode);

            if (!currency.InUse)
                throw new ArgumentException($"Currency {currencyCode} is not in use");

            if (decimal.Round(amount, currency.DecimalPlaces) != amount)
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    $"Amount in {currencyCode} cannot have more than {currency.DecimalPlaces} decimals"
                );

            Amount = amount;
            Currency = currency;
        }

        protected Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected Money() {}

        public static Money FromDecimal(
            decimal amount,
            string currency,
            ICurrencyLookup currencyLookup) 
            => new Money(amount, currency, currencyLookup);

        public static Money FromString(
            string amount,
            string currency,
            ICurrencyLookup currencyLookup)
            => new Money(decimal.Parse(amount), currency, currencyLookup);
        
        public Money Add(Money sum)
        {
            if (Currency != sum.Currency)
                throw new CurrencyMismatchException(
                    "Cannot sum amounts with different currencies"
                );
            
            return new Money (Amount + sum.Amount, Currency);
        }

        public Money Substract(Money sum)
        {
            if (Currency != sum.Currency)
                throw new CurrencyMismatchException(
                    "Cannot substract amounts with different currencies"
                );
            
            return new Money (Amount - sum.Amount, Currency);
        }

        public static Money operator +(Money sum1, Money sum2) => sum1.Add(sum2);
        public static Money operator -(Money sum1, Money sum2) => sum1.Substract(sum2);
        public override string ToString() => $"{Currency.CurrencyCode} {Amount}";
    }

    public class CurrencyMismatchException : Exception 
    {
        public CurrencyMismatchException(string message) : base(message) {}
    }
}