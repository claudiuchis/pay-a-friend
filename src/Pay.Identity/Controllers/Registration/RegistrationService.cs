using System;
using Eventuous;
using Pay.Identity.Domain.Users;
using static Pay.Identity.Registration.Commands;

namespace Pay.Identity.Registration
{
    public class RegistrationService : ApplicationService<User, UserState, UserId>
    {
        public RegistrationService(
            IAggregateStore store,
            Func<string, string> hashPassword
            ) : base(store) 
        {
            OnNew<V1.RegisterUser>(
                cmd => new UserId(cmd.UserId),
                (user, cmd)
                    => user.Register(
                        new UserId(cmd.UserId),
                        new Email(cmd.Email),
                        HashedPassword.FromString(cmd.Password, hashPassword),
                        new FullName(cmd.FullName)
                    )
            );
        }
    }
}