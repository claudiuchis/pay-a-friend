using System;
using Eventuous;

namespace Pay.Identity.Domain.Users
{
    public record UserId(string Value) : AggregateId(Value);
}