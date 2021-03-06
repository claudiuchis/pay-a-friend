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
            OnAny<V1.CreateTransferOrder>(
                cmd => new TransferOrderId(cmd.TransferOrderId),
                (order, cmd) => order.CreateTransferOrder(
                    new TransferOrderId(cmd.TransferOrderId),
                    new PrepaidAccountId(cmd.PayorPrepaidAccountId),
                    new PrepaidAccountId(cmd.PayeePrepaidAccountId),
                    Funds.FromDecimal(cmd.Amount, cmd.CurrencyCode, currencyLookup)
                )
            );

            OnExisting<V1.CompleteTransferOrder>(
                cmd => new TransferOrderId(cmd.TransferOrderId),
                (order, cmd) => order.CompleteOrder(
                    new TransferOrderId(cmd.TransferOrderId)
                )
            );

            OnExisting<V1.FailTranferOrder>(
                cmd => new TransferOrderId(cmd.TransferOrderId),
                (order, cmd) => order.FailTranferOrder(
                    new TransferOrderId(cmd.TransferOrderId),
                    cmd.Stage,
                    cmd.Reason
                )
            );

        }

    }
}