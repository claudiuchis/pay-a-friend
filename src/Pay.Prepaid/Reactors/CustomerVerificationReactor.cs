using System;
using System.Threading;
using System.Threading.Tasks;
using Eventuous.Subscriptions;

using static Pay.Prepaid.Reactors.IntegrationEvents;
using Pay.Prepaid.PrepaidAccounts;

namespace Pay.Prepaid.Reactors
{
    public class CustomerVerificationReactor : IEventHandler
    {
        public string SubscriptionId { get; }
        PrepaidAccountsCommandService _prepaidAccountsCommandService;
        public CustomerVerificationReactor(
            string subscriptionGroup,
            PrepaidAccountsCommandService prepaidAccountsCommandService)
        {
            SubscriptionId = subscriptionGroup;
            _prepaidAccountsCommandService = prepaidAccountsCommandService;
        }
        public async Task HandleEvent(object @event, long? position, CancellationToken cancellationToken)
        {
            var result = @event switch 
            {
                V1.CustomerVerified verified => 
                    _prepaidAccountsCommandService.Handle(
                        new Commands.V1.CreatePrepaidAccount(
                            Guid.NewGuid().ToString(),
                            verified.CustomerId,
                            verified.CurrencyCode
                        ),
                        cancellationToken
                    ),
                _ => Task.CompletedTask
            };
            await result;
        }  
    }
}