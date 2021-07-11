using System;
using System.Threading.Tasks;
using Eventuous.Subscriptions;
using static Pay.Prepaid.Reactors.IntegrationEvents;
using Pay.Prepaid.PrepaidAccounts;

namespace Pay.Prepaid.Reactors
{
    public class TopUpReactor : IEventHandler
    {
        public string SubscriptionId { get; }
        PrepaidAccountService _prepaidAccountService;
        public TopUpReactor(
            string subscriptionGroup,
            PrepaidAccountService prepaidAccountService)
        {
            SubscriptionId = subscriptionGroup;
            _prepaidAccountService = prepaidAccountService;
        }
        Task HandleEvent(object @event, long? position, CancellationToken cancellationToken)
        {
            @event switch 
            {
                V1.TopUpCompleted completed => 
                    _prepaidAccountService.Handle(
                        new CreditPrepaidAccount(
                            Guid.NewGuid().ToString(),
                            CustomerId,
                            
                    ))
            }
        }  
    }
}