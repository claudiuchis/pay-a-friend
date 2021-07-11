using System;
using System.Linq;
using System.Threading.Tasks;
using Eventuous;
using static Pay.TopUps.StripePayments.Events;

namespace Pay.TopUps.StripePayments
{
    public class StripeWebhookService
    {
        IEventStore _eventStore;
        public StripeWebhookService(IEventStore eventStore)
            => _eventStore = eventStore;

        public async Task React(Stripe.Event stripeEvent)
        {
            if (stripeEvent.Type == Stripe.Events.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as Stripe.PaymentIntent;
                var integrationEvent = new V1.TopUpCompleted(
                    paymentIntent.Id,
                    paymentIntent.Amount / 100, // amount converted from cents to 2 decimal places
                    paymentIntent.Currency,
                    paymentIntent.CustomerId,
                    paymentIntent.PaymentMethod.Type,
                    paymentIntent.PaymentMethod.Card.Last4
                );
                await EmitTopUpCompletedEvent(integrationEvent);        
            }                
        }

        public async Task EmitTopUpCompletedEvent(V1.TopUpCompleted e)
        {
            await _eventStore.AppendEvents(
                StreamNames.TopUpsStream, 
                ExpectedStreamVersion.Any,
                ToStreamEvents(new object[] {e}),
                default
            );
        }

        StreamEvent[] ToStreamEvents(object[] events)
            => events.Select<object, StreamEvent>( @event => new(
                TypeMap.GetTypeName(@event),
                DefaultEventSerializer.Instance.Serialize(@event),
                null,
                DefaultEventSerializer.Instance.ContentType
            )).ToArray();
    }
}
