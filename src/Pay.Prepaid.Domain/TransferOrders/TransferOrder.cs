using System;
using System.Threading.Tasks;
using Eventuous;
using Pay.Prepaid.Domain.PrepaidAccounts;
using static Pay.Prepaid.Domain.TransferOrders.Events;

namespace Pay.Prepaid.Domain.TransferOrders
{
    public class TransferOrder : Aggregate<TransferOrderState, TransferOrderId>
    {
        public void CreateTransferOrder(
            TransferOrderId orderId,
            PrepaidAccountId payorAccountId,
            PrepaidAccountId payeeAccountId,
            Funds funds
        )
        {
            EnsureDoesntExist();
            Apply(new V1.TransferOrderCreated(
                orderId,
                payorAccountId,
                payeeAccountId,
                funds.Currency.CurrencyCode,
                funds.Amount
            ));
        }

        public void CompleteOrder(
            TransferOrderId orderId
        )
        {
            Apply(new V1.TransferOrderCompleted(
                orderId
            ));
        }
        public void FailTranferOrder(
            TransferOrderId orderId,
            string stage,
            string reason
        )
        {
            Apply(new V1.TransferOrderFailed(
                orderId,
                stage,
                reason
            ));
        }

    }

    public record TransferOrderState : AggregateState<TransferOrderState, TransferOrderId>
    {
        public enum TransferOrderStatus
        {
            NotStarted,
            OrderPlaced,
            OrderCompleted,
            OrderFailed
        };

        public TransferOrderState(){
            OrderStatus = TransferOrderStatus.NotStarted;
        }   

        public TransferOrderStatus OrderStatus { get; init; }
        public PrepaidAccountId PayorAccountId { get; init; }
        public PrepaidAccountId PayeeAccountId { get; init; }
        public string Reason { get; init; }
        public string Stage { get; init; }

        public override TransferOrderState When(object @event)
            => @event switch {
                V1.TransferOrderCreated created => 
                    OrderStatus switch {
                        TransferOrderStatus.NotStarted => this with {
                            Id = new TransferOrderId(created.TransferOrderId),
                            PayeeAccountId = new PrepaidAccountId(created.PayeePrepaidAccountId),
                            PayorAccountId = new PrepaidAccountId(created.PayorPrepaidAccountId),
                            OrderStatus = TransferOrderStatus.OrderPlaced
                        },
                        _ => this
                    },
                V1.TransferOrderCompleted completed => 
                    OrderStatus switch {
                        TransferOrderStatus.OrderPlaced => this with {
                            OrderStatus = TransferOrderStatus.OrderCompleted
                        },
                        _ => this
                    },
                V1.TransferOrderFailed failed => 
                    OrderStatus switch {
                        TransferOrderStatus.OrderPlaced => this with {
                            OrderStatus = TransferOrderStatus.OrderFailed,
                            Reason = failed.Reason
                        },
                        _ => this
                    },
                _ => this
            };
    }
}
