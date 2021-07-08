using System;
using System.Threading.Tasks;
using Eventuous;
using static Pay.TopUps.Domain.Events;

namespace Pay.TopUps.Domain
{
    public class TopUp : Aggregate<TopUpState, TopUpId>
    {
        public async Task SubmitTopUp(
            IPaymentService paymentService,
            TopUpId topUpId, 
            CardDetails card, 
            BillingDetails billing, 
            TopUpAmount topUpAmount)
        {
            var result = await paymentService.ChargeCard(card, billing, topUpAmount);
            if (String.IsNullOrWhiteSpace(result.PaymentId))
            {
                Apply(new V1.TopUpFailed(
                    topUpId, 
                    topUpAmount.Amount, 
                    topUpAmount.Currency.CurrencyCode, 
                    result.PaymentProvider,
                    result.Reason,
                    result.CardLast4Digits
                ));
            }
            else
            {
                Apply(new V1.TopUpCompleted(
                    topUpId, 
                    topUpAmount.Amount, 
                    topUpAmount.Currency.CurrencyCode, 
                    result.PaymentProvider,
                    result.PaymentId, 
                    result.CardLast4Digits
                ));
            }
        }
    }

    public record TopUpState : AggregateState<TopUpState, TopUpId>
    {
        TopUpAmount Amount { get; init; }
        PaymentResult Result { get; init; }
        public override TopUpState When(object @event)
            => @event switch {
                V1.TopUpCompleted completed => this with {
                    Id = new TopUpId(completed.TopUpId),
                    Amount = new TopUpAmount(completed.Amount, completed.CurrencyCode),
                    Result = new PaymentResult(new PaymentProvider(completed.PaymentProvider), completed.CardLast4Digits) {
                        PaymentId = completed.PaymentId
                    }
                },
                V1.TopUpFailed failed => this with {
                    Id = new TopUpId(failed.TopUpId),
                    Amount = new TopUpAmount(failed.Amount, failed.CurrencyCode),
                    Result = new PaymentResult(new PaymentProvider(failed.PaymentProvider), failed.CardLast4Digits) {
                        Reason = failed.Reason
                    }
                },
                _ => this
            };
    }
}

