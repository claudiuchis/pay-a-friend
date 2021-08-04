using System;
using Eventuous;
using Pay.Identity.Domain.Users;
using Pay.Identity.Domain.Emails;
using static Pay.Identity.Registration.Commands;

namespace Pay.Identity.Registration
{
    public class RegistrationService : ApplicationService<User, UserState, UserId>
    {
        public RegistrationService(
            IAggregateStore store,
            Func<string, string> hashPassword,
            ISendEmailService emailService
            ) : base(store) 
        {
            OnAny<V1.RegisterUser>(
                cmd => new UserId(cmd.UserId),
                (user, cmd)
                    => user.Register(
                        new UserId(cmd.UserId),
                        new Email(cmd.Email),
                        HashedPassword.FromString(cmd.Password, hashPassword),
                        new FullName(cmd.FullName)
                    )
            );

            OnExisting<V1.SendConfirmationEmail>(
                cmd => new UserId(cmd.UserId),
                (user, cmd)
                    => user.SendConfirmationEmail(
                        emailService
                    )
            );

            OnExisting<V1.ConfirmEmail>(
                cmd => new UserId(cmd.UserId),
                (user, cmd)
                    => user.ConfirmEmail(
                        cmd.Token
                    )
            );
        }
    }
}