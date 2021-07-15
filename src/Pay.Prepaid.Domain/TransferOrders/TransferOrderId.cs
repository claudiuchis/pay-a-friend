using Eventuous;

namespace Pay.Prepaid.Domain.TransferOrders
{
    public record TransferOrderId (string Value) : AggregateId(Value);
}