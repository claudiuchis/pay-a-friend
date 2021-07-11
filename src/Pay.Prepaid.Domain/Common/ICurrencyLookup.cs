namespace Pay.Prepaid.Domain
{
    public interface ICurrencyLookup
    {
        Currency FindCurrency(string currencyCode);
    }
}