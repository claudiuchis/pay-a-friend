using System;
using System.Threading;
using System.Threading.Tasks;
using Eventuous.Subscriptions;

using Pay.Prepaid.PrepaidAccounts;
using static Pay.Prepaid.PrepaidAccounts.Commands.V1;
using static Pay.Prepaid.TransferOrders.Commands.V1;
using Pay.Prepaid.Domain.PrepaidTransactions;
using static Pay.Prepaid.Domain.PrepaidAccounts.Events.V1;
using static Pay.Prepaid.Domain.TransferOrders.Events.V1;
using Pay.Prepaid.TransferOrders;

namespace Pay.Prepaid.Reactors
{
    public class TransferReactor : IEventHandler
    {
        public string SubscriptionId { get; }
        TransferOrdersCommandService _transferOrdersCommandService;
        PrepaidAccountsCommandService _prepaidAccountsCommandService;
        TransferOrdersQueryService _transferOrdersQueryService;

        public TransferReactor(
            string subscriptionGroup,
            TransferOrdersCommandService transferOrdersCommandService,
            PrepaidAccountsCommandService prepaidAccountsCommandService,
            TransferOrdersQueryService transferOrdersQueryService
        )
        {
            SubscriptionId = subscriptionGroup;
            _transferOrdersCommandService = transferOrdersCommandService;
            _prepaidAccountsCommandService = prepaidAccountsCommandService;
            _transferOrdersQueryService = transferOrdersQueryService;
        }

        public async Task HandleEvent(object @event, long? position, CancellationToken cancellationToken)
        {
            switch (@event)
            {
                case TransferOrderCreated orderCreated:
                {
                    try 
                    {
                        await _prepaidAccountsCommandService.Handle(
                            new PlaceHoldOnPrepaidAccount(
                                orderCreated.PayorPrepaidAccountId,
                                orderCreated.Amount,
                                orderCreated.CurrencyCode,
                                PrepaidTransactionType.TransferBetweenAccounts,
                                orderCreated.TransferOrderId
                            ),
                            cancellationToken
                        );
                    }
                    catch( Exception e)
                    {
                        await _transferOrdersCommandService.Handle(
                            new FailTranferOrder(
                                orderCreated.TransferOrderId,
                                "PlaceHoldOnPrepaidAccount",
                                e.Message
                            ),
                            cancellationToken
                        );
                    }
                }
                break;

                case PrepaidAccountHoldPlaced holdPlaced:
                {
                    var transfer = await _transferOrdersQueryService.GetTransferOrder(holdPlaced.TransactionId);
                    try
                    {
                        await _prepaidAccountsCommandService.Handle(
                            new CreditPrepaidAccount(
                                transfer.PayorPrepaidAccountId,
                                transfer.Amount,
                                transfer.CurrencyCode,
                                PrepaidTransactionType.TransferBetweenAccounts,
                                transfer.TransferOrderId
                            ),
                            cancellationToken
                        );
                    }
                    catch(Exception e)
                    {
                        await _prepaidAccountsCommandService.Handle(
                            new ReleaseHoldOnPrepaidAccount(
                                transfer.PayorPrepaidAccountId,
                                transfer.TransferOrderId,
                                e.Message
                            ),
                            cancellationToken
                        );
                    }
                }
                break;

                case PrepaidAccountHoldReleased released:
                {
                    var transfer = await _transferOrdersQueryService.GetTransferOrder(released.TransactionId);
                    await _transferOrdersCommandService.Handle(
                        new FailTranferOrder(
                            transfer.TransferOrderId,
                            "PrepaidAccountHoldReleased",
                            released.Reason
                        ),
                        cancellationToken
                    );
                }
                break;

                case PrepaidAccountCredited credited:
                {
                    var transfer = await _transferOrdersQueryService.GetTransferOrder(credited.TransactionId);

                    await _prepaidAccountsCommandService.Handle(
                        new DebitPrepaidAccount(
                            transfer.PayorPrepaidAccountId,
                            transfer.Amount,
                            transfer.CurrencyCode,
                            PrepaidTransactionType.TransferBetweenAccounts,
                            transfer.TransferOrderId
                        ),
                        cancellationToken
                    );
                }
                break;

                case PrepaidAccountDebited accountDebited:
                {
                    if (accountDebited.TransactionType.Equals(PrepaidTransactionType.TransferBetweenAccounts))
                    {
                        await _transferOrdersCommandService.Handle(
                            new CompleteTransferOrder(
                                accountDebited.TransactionId
                            ),
                            cancellationToken
                        );
                    }
                }
                break;
                
            };
        }

    }
}