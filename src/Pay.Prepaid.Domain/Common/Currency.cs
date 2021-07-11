namespace Pay.Prepaid.Domain
{
    public record Currency
    {
        public static Currency None = new Currency {InUse = false};
        public string CurrencyCode { get; set; }
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }
        public static implicit operator string(Currency self) => self.CurrencyCode;
    }

}