using System;

namespace Pay.Prepaid.Domain
{
    public record Money {

        protected Money(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected Money(
            decimal amount, 
            string currencyCode,
            ICurrencyLookup currencyLookup)
        {
            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentNullException(
                    nameof(currencyCode), "Currency code must be specified"
                );

            var currency = currencyLookup.FindCurrency(currencyCode);

            if (!currency.InUse)
                throw new ArgumentException($"Currency {currencyCode} is not valid");

            if (decimal.Round(amount, currency.DecimalPlaces) != amount)
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    $"Amount in {currencyCode} cannot have more than {currency.DecimalPlaces} decimals"
                );

            Amount = amount;
            Currency = currency;
        }
        public decimal Amount { get; }
        public Currency Currency { get; }

        public static Money FromDecimal(
            decimal amount,
            string currency,
            ICurrencyLookup currencyLookup)
            => new Money(amount, currency, currencyLookup);

        public Money Add(Money summand)
        {
            if (Currency != summand.Currency)
                throw new CurrencyMismatchException(
                    $"Cannot sum amounts with different currencies ({Currency} vs {summand.Currency})"
                );

            return new Money(Amount + summand.Amount, Currency);
        }

        public Money Subtract(Money subtrahend)
        {
            if (Currency != subtrahend.Currency)
                throw new CurrencyMismatchException(
                    "Cannot subtract amounts with different currencies ({Currency} vs {subtrahend.Currency})"
                );

            return new Money(Amount - subtrahend.Amount, Currency);
        }

        public bool Compare(Money compareEnd)
        {
            if (Currency != compareEnd.Currency)
                throw new CurrencyMismatchException(
                    "Cannot compare amounts with different currencies ({Currency} vs {compareEnd.Currency})"
                );
            return (Amount - compareEnd.Amount > 0); 
        }

        public static Money operator +(Money summand1, Money summand2) => summand1.Add(summand2);

        public static Money operator -(Money minuend, Money subtrahend) => minuend.Subtract(subtrahend);
        public static bool operator <(Money left, Money right) => left.Compare(right); 
        public static bool operator >(Money left, Money right) => !left.Compare(right); 

        public override string ToString() => $"{Currency.CurrencyCode} {Amount}";
    }
    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) : base(message)
        {
        }
    }

}