using System;
using System.Threading;
using System.Threading.Tasks;
using Eventuous.Subscriptions;

using static Pay.Prepaid.Reactors.IntegrationEvents;
using Pay.Prepaid.PrepaidAccounts;

namespace Pay.Prepaid.Reactors
{
    public class CustomerReactor : IEventHandler
    {
        public string SubscriptionId { get; }
        PrepaidAccountsCommandService _prepaidAccountsCommandService;
        public CustomerReactor(
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
                V1.CustomerDetailsVerified verified => 
                    _prepaidAccountsCommandService.Handle(
                        new Commands.V1.CreatePrepaidAccount(
                            Guid.NewGuid().ToString(),
                            verified.CustomerId,
                            verified.CountryCode
                        ),
                        cancellationToken
                    ),
                _ => Task.CompletedTask
            };
            await result;
        }  
    }
}