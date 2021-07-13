namespace Pay.Prepaid.Domain.Shared
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
        Currency FindCurrencyByCountry(string countryCode);
    }
}