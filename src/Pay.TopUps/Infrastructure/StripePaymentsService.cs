using System.Threading.Tasks;
using Pay.TopUps.Domain;
using Stripe;
using AutoMapper;

namespace Pay.TopUps.Infrastructure
{
    public class StripePaymentsService : IPaymentService
    {
        IMapper _mapper; 
        public StripePaymentsService(IMapper mapper) => _mapper = mapper;
        public async Task<PaymentResult> ChargeCard(CardDetails card, BillingDetails billing, Money amount)
        {
            var source = _mapper.Map<CardCreateNestedOptions>(card)
                .Map(billing, _mapper);

            var options = new ChargeCreateOptions
            {
                Amount = (long)amount.Amount,
                Currency = amount.Currency.CurrencyCode,
                Source = source
            };

            var service = new ChargeService();
            var charge = await service.CreateAsync(options);
            if (charge.Paid)
            {
                return new PaymentResult(PaymentProvider.Stripe, charge.PaymentMethodDetails.Card.Last4) {
                    PaymentId = charge.Id
                };
            }
            else 
            {
                return new PaymentResult(PaymentProvider.Stripe, charge.PaymentMethodDetails.Card.Last4) {
                    Reason = charge.Outcome.Reason
                };
            }
        }
    }
}