using System;
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
            Apply(new V1.TransferOrderCreated(
                orderId,
                payorAccountId,
                payeeAccountId,
                funds.Currency.CurrencyCode,
                funds.Amount
            ));
        }

        public void AcknowledgePayorAccountDebited(
            TransferOrderId orderId
        )
        {
            Apply(new V1.PayorAccountDebited(
                orderId
            ));
        }

        public void AcknowledgePayeeAccountCredited(
            TransferOrderId orderId
        )
        {
            Apply(new V1.PayeeAccountCredited(
                orderId
            ));
        }

        public void AcknowledgeOrderFailed(
            TransferOrderId orderId,
            string reason
        )
        {
            Apply(new V1.OrderFailed(
                orderId,
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
            PayorAccountDebited,
            PayeeAccountCredited,
            OrderFailed
        };

        public TransferOrderState(){
            OrderStatus = TransferOrderStatus.NotStarted;
        }   

        public TransferOrderStatus OrderStatus { get; init; }
        public PrepaidAccountId PayorAccountId { get; init; }
        public PrepaidAccountId PayeeAccountId { get; init; }
        public string Reason { get; init; }

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
                V1.PayorAccountDebited debited => 
                    OrderStatus switch {
                        TransferOrderStatus.OrderPlaced => this with {
                            OrderStatus = TransferOrderStatus.PayorAccountDebited
                        },
                        _ => this
                    },
                V1.PayeeAccountCredited credited => 
                    OrderStatus switch {
                        TransferOrderStatus.PayorAccountDebited => this with {
                            OrderStatus = TransferOrderStatus.PayeeAccountCredited
                        },
                        _ => this
                    },
                V1.OrderFailed failed => this with {
                    OrderStatus = TransferOrderStatus.OrderFailed,
                    Reason = failed.Reason
                },
                _ => this
            };
    }
}
