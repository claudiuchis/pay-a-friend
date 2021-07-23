namespace Pay.Prepaid.Domain.TransferOrders
{
    public record PrepaidAccount(
        string PrepaidAccountId,
        decimal Balance,
        string CurrencyCode
    );
}