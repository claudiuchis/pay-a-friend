using System;
using Eventuous;
using static App.Identity.Domain.Users.Events;

namespace App.Identity.Domain.Users
{
    public class User : Aggregate<UserState, UserId> {
        public void Register(UserId id, Email email, HashedPassword hashedPassword, FullName fullName) {
            EnsureDoesntExist();
            Apply(new V1.UserRegistered(id, email, hashedPassword, fullName));
        }
    }
    public record UserState : AggregateState<UserState, UserId> {
        Email Email { get; init; }
        FullName FullName { get; init; }
        string HashedPassword { get; init; }
        public override UserState When(object @event)
            => @event switch {
                V1.UserRegistered registered => this with { 
                    Id = new UserId(registered.UserId), 
                    Email = new Email(registered.Email), 
                    HashedPassword = registered.EncryptedPassword, 
                    FullName = new FullName(registered.FullName)},
                _ => this
            };
    }
}