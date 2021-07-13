using System;
using Eventuous;

namespace Pay.Verification.Domain
{
    public record CustomerId(string Value) : AggregateId(Value);
}