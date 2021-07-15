using System.Threading;
using System.Threading.Tasks;
using Eventuous.Subscriptions;
using static Pay.Prepaid.Domain.PrepaidAccounts.Events;
using Pay.Prepaid.Domain.TransferOrders;
using Pay.Prepaid.TransferOrders;
using Pay.Prepaid.PrepaidAccounts;

namespace Pay.Prepaid.Reactors
{
    public class PrepaidReactor : IEventHandler
    {
        public string SubscriptionId { get; }
        TransferOrdersCommandService _transferOrdersCommandService;
        PrepaidAccountsCommandService _prepaidAccountsCommandService;

        public PrepaidReactor(
            string subscriptionGroup,
            TransferOrdersCommandService transferOrdersCommandService,
            PrepaidAccountsCommandService prepaidAccountsCommandService
        )
        {
            SubscriptionId = subscriptionGroup;
            _transferOrdersCommandService = transferOrdersCommandService;
            _prepaidAccountsCommandService = prepaidAccountsCommandService;
        }

        public async Task HandleEvent(object @event, long? position, CancellationToken cancellationToken)
        {
            var result = @event switch 
            {
                Events.V1.TransferOrderCreated orderCreated => 
                    _prepaidAccountsCommandService.Handle
            }
        }

    }
}