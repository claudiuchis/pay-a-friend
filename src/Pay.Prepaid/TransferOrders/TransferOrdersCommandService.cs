using Eventuous;
using Pay.Prepaid.Domain.TransferOrders;
using Pay.Prepaid.Domain.PrepaidAccounts;
using Pay.Prepaid.Domain.Shared;
using static Pay.Prepaid.TransferOrders.Commands;

namespace Pay.Prepaid.TransferOrders
{
    public class TransferOrdersCommandService
        : ApplicationService<TransferOrder, TransferOrderState, TransferOrderId>
    {
        public TransferOrdersCommandService(
            IAggregateStore store,
            ICurrencyLookup currencyLookup
        ) : base (store)
        {
            OnNew<V1.CreateTransferOrder>(
                cmd => new TransferOrderId(cmd.TransferOrderId),
                (order, cmd) => order.CreateTransferOrder(
                    new TransferOrderId(cmd.TransferOrderId),
                    new PrepaidAccountId(cmd.PayorPrepaidAccountId),
                    new PrepaidAccountId(cmd.PayeePrepaidAccountId),
                    Funds.FromDecimal(cmd.Amount, cmd.CurrencyCode, currencyLookup)
                )
            );

            OnExisting<V1.AcknowlegePayorAccountDebited>(
                cmd => new TransferOrderId(cmd.TransferOrderId),
                (order, cmd) => order.AcknowledgePayorAccountDebited(
                    new TransferOrderId(cmd.TransferOrderId)
                )
            );

            OnExisting<V1.AcknowledgePayeeAccountCredited>(
                cmd => new TransferOrderId(cmd.TransferOrderId),
                (order, cmd) => order.AcknowledgePayeeAccountCredited(
                    new TransferOrderId(cmd.TransferOrderId)
                )
            );

            OnExisting<V1.AcknowledgeOrderFailed>(
                cmd => new TransferOrderId(cmd.TransferOrderId),
                (order, cmd) => order.AcknowledgeOrderFailed(
                    new TransferOrderId(cmd.TransferOrderId),
                    cmd.Reason
                )
            );

        }

    }
}