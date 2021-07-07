using Eventuous;
using static Pay.TopUps.Domain.Events;

namespace Pay.TopUps.Domain
{
    public class TopUp : Aggregate<TopUpState, TopUpId>
    {
        public void SubmitTopUp(TopUpId topUpId, TopUpAmount topUpAmount)
        {
            Apply(new V1.TopUpCompleted(topUpId, topUpAmount.Amount, topUpAmount.Currency.CurrencyCode));
        }
    }

    public record TopUpState : AggregateState<TopUpState, TopUpId>
    {
        TopUpAmount Amount { get; init; }
        public override TopUpState When(object @event)
            => @event switch {
                V1.TopUpCompleted completed => this with {
                    Id = new TopUpId(completed.TopUpId),
                    Amount = new TopUpAmount(completed.Amount, completed.CurrencyCode)
                },
                _ => this
            };
    }
}

