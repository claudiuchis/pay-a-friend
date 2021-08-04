using System;
using System.Threading.Tasks;
using Eventuous;
using static Pay.Identity.Domain.Users.Events;
using Pay.Identity.Domain.Emails;

namespace Pay.Identity.Domain.Users
{
    public class User : Aggregate<UserState, UserId> {
        public void Register(UserId id, Email email, HashedPassword hashedPassword, FullName fullName) {
            EnsureDoesntExist();
            Apply(new V1.UserRegistered(id, email, hashedPassword, fullName));
        }

        public async Task SendConfirmationEmail(ISendEmailService emailService)
        {
            var token = new EmailConfirmationToken(
                Guid.NewGuid().ToString(), 
                DateTime.Now.AddDays(1)
            );
            
            await emailService.SendEmailConfirmationEmail(
                GetId(),
                State.Email, 
                State.FullName, 
                token.Token.ToString()
            );
            Apply(new V1.ConfirmationEmailSent(token.Token.ToString(), token.ValidTo.ToString()));
        }

        public void ConfirmEmail(string token)
        {
            if (State.ConfirmationToken != null && State.ConfirmationToken.IsTokenValid(token))
            {
                Apply(new V1.EmailConfirmed());
            }
            throw new EmailConfirmationTokenInvalidException("The token included in the email confirmation link is invalid");
        }
    }
    public record UserState : AggregateState<UserState, UserId> {
        
        public Email Email { get; init; }
        public FullName FullName { get; init; }
        string HashedPassword { get; init; }
        public EmailConfirmationToken ConfirmationToken { get; init; }
        public bool EmailConfirmed { get; set; }

        public override UserState When(object @event)
            => @event switch {
                V1.UserRegistered registered => this with { 
                    Id = new UserId(registered.UserId), 
                    Email = new Email(registered.Email), 
                    HashedPassword = registered.EncryptedPassword, 
                    FullName = new FullName(registered.FullName),
                    EmailConfirmed = false
                },
                
                V1.ConfirmationEmailSent sent => this with {
                    ConfirmationToken = new EmailConfirmationToken(sent.Token, DateTime.Parse(sent.ValidTo))
                },

                V1.EmailConfirmed confirmed => this with {
                    EmailConfirmed = true
                },

                _ => this
            };
    }
}