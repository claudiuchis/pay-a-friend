namespace Pay.Prepaid.Domain.Shared
{
    public record Currency
    {
        public static Currency None = new Currency {InUse = false};
        public string CurrencyCode { get; set; }
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }
        public string[] CountriesInUse { get; set; }
        public static implicit operator string(Currency self) => self.CurrencyCode;
    }

}