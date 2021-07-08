using System.Threading.Tasks;

namespace Pay.TopUps.Domain
{
    public interface IPaymentService
    {
        Task<PaymentResult> ChargeCard(CardDetails card, BillingDetails billing, Money amount);
    }
}