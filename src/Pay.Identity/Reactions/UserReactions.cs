using System;
using System.Threading;
using System.Threading.Tasks;
using Eventuous.Subscriptions;

using static Pay.Identity.Registration.Commands.V1;
using static Pay.Identity.Domain.Users.Events;
using Pay.Identity.Registration;

namespace Pay.Identity.Reactions
{
    public class UserReactions : IEventHandler
    {
        private RegistrationService _registrationService;
        public string SubscriptionId { get; }
        public UserReactions(
            string subscriptionGroup,
            RegistrationService registrationService
        )
        {
            SubscriptionId = subscriptionGroup;
            _registrationService = registrationService;
        }

        public async Task HandleEvent(
            object @event, 
            long? position,
            CancellationToken cancellationToken
        )
        {
            var result = @event switch
            {
                V1.UserRegistered registered => _registrationService.Handle(
                    new SendConfirmationEmail(registered.UserId),
                    cancellationToken
                ),
                _ => Task.CompletedTask
            };

            await result;
        }
    }
}