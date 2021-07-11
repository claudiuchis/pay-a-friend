using Eventuous;
namespace Pay.Prepaid.Domain
{
    public record PrepaidAccountId(string Value) : AggregateId(Value);
}