using System;
using Eventuous;

namespace App.Identity.Domain.Users
{
    public record UserId(string Value) : AggregateId(Value);
}