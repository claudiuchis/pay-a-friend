using Eventuous;

namespace Pay.TopUps.Domain
{
    public record TopUpId(string Value): AggregateId(Value);
}