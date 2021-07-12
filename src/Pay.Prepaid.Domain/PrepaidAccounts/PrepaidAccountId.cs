using Eventuous;
namespace Pay.Prepaid.Domain.PrepaidAccounts
{
    public record PrepaidAccountId(string Value) : AggregateId(Value);
}