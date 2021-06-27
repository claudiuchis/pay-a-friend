using System;
using Eventuous;

namespace Pay.Verification.Domain
{
    public record VerificationDetailsId(string Value) : AggregateId(Value);
}