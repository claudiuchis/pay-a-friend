using System;
using System.Threading;
using System.Threading.Tasks;
using Eventuous.Subscriptions;
using Microsoft.AspNetCore.Mvc;

using static Pay.Prepaid.Reactors.IntegrationEvents;
using Pay.Prepaid.PrepaidAccounts;
using Pay.Prepaid.Domain.PrepaidTransactions;

namespace Pay.Prepaid.Reactors
{
    public class TopUpReactor : IEventHandler
    {
        public string SubscriptionId { get; }
        PrepaidAccountsCommandService _prepaidAccountsCommandService;
        PrepaidAccountsQueryService _prepaidAccountsQueryService;

        public TopUpReactor(
            string subscriptionGroup,
            PrepaidAccountsCommandService prepaidAccountsCommandService,
            PrepaidAccountsQueryService prepaidAccountsQueryService)
        {
            SubscriptionId = subscriptionGroup;
            _prepaidAccountsCommandService = prepaidAccountsCommandService;
            _prepaidAccountsQueryService = prepaidAccountsQueryService;
        }
        public async Task HandleEvent(object @event, long? position, CancellationToken cancellationToken)
        {
            var result = @event switch 
            {
                V1.TopUpCompleted completed => 
                    _prepaidAccountsCommandService.Handle(
                        new Commands.V1.CreditPrepaidAccount(
                            _prepaidAccountsQueryService.GetPrepaidAccountForCustomer(completed.CustomerId),
                            completed.Amount,
                            completed.CurrencyCode,
                            PrepaidTransactionType.TopUp,
                            completed.TransactionId
                        ),
                        cancellationToken
                    ),
                _ => Task.CompletedTask
            };
            await result;
        }
    }
}