using System.Threading;
using Pay.TopUps.Domain;
using Stripe;
using Automapper;

namespace Pay.TopUps.Infrastructure
{
    public class StripePaymentsService : IPaymentService
    {
        async Task<PaymentResult> ChargeCard(CardDetails card, BillingDetails billing, Money amount)
        {
            var source = Mapper.Map<CardCreateNestedOptions>(card)
                .Map(billing);
            source.Amount = amount.Amount;
            source.Currency = amount.Currency.CurrencyCode;

            var options = new ChargeCreateOptions
            {
                Amount = amount.Amount,
                Currency = amount.Currency.CurrencyCode,
                Source = source
            };

            var service = new ChargeService();
            var charge = await service.CreateAsync(options);
            if (charge.Paid)
            {
                return new PaymentResult {
                    Provider = PaymentResult.PaymentProviders.Stripe,
                    PaymentId = charge.Id,
                    CardLast4Digits = charge.PaymentMethodDetails.Card.Last4
                };
            }
            else 
            {
                return new PaymentResult {
                    Provider = PaymentResult.PaymentProviders.Stripe,
                    Reason = charge.Outcome.Reason,
                    CardLast4Digits = charge.PaymentMethodDetails.Card.Last4
                };
            }
        }
    }
}