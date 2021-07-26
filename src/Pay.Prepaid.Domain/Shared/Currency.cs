using System;
namespace Pay.Prepaid.Domain.Shared
{
    public sealed record Currency
    {
        public static Currency None = new Currency {InUse = false};
        public string CurrencyCode { get; set; }
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }
        public string[] CountriesInUse { get; set; }
        public static implicit operator string(Currency self) => self.CurrencyCode;
        public bool Compare(Currency compareEnd)
            => CurrencyCode.Equals(compareEnd, StringComparison.OrdinalIgnoreCase);

        public bool Equals(Currency other) => this.Compare(other);

        public override int GetHashCode() => CurrencyCode.GetHashCode();
    }

}