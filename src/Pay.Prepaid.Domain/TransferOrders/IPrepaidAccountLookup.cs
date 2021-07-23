using System.Threading.Tasks;

namespace Pay.Prepaid.Domain.TransferOrders
{
    public interface IPrepaidAccountLookup
    {
        Task<PrepaidAccount> GetPrepaidAccount(string prepaidAccountId);
    }
}