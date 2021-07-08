using System;
using Eventuous;
using static Pay.TopUps.Domain.Events;

namespace Pay.TopUps.Domain
{
    public class TopUp : Aggregate<TopUpState, TopUpId>
    {
        IPaymentService _paymentService;
        public TopUp(IPaymentService paymentService) => _paymentService = paymentService;
        public async Task SubmitTopUp(TopUpId topUpId, CardDetails card, TopUpAmount topUpAmount)
        {
            result = await _paymentService.ChargeCard(card, topUpAmount);
            if (String.IsNullOrWhiteSpace(result.PaymentId))
            {
                Apply(new V1.TopUpFailed(topUpId, topUpAmount.Amount, topUpAmount.Currency.CurrencyCode, result.PaymentId, PaymentResult.PaymentProvider));
            }
            else
            {
                Apply(new V1.TopUpCompleted(topUpId, topUpAmount.Amount, topUpAmount.Currency.CurrencyCode, result.PaymentId, PaymentResult.PaymentProvider));
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
                    Result = new PaymentResult(completed.PaymentProvider, completed.PaymentId, completed.CardLast4Digits)
                },
                V1.TopUpFailed failed => this with {
                    Id = new TopUpId(failed.TopUpId),
                    Amount = new TopUpAmount(failed.Amount, failed.CurrencyCode),
                    Result = new PaymentResult(failed.PaymentProvider, failed.Reason, failed.CardLast4Digits)
                },
                _ => this
            };
    }
}

