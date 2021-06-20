using System;
using Eventuous;
using App.Identity.Domain.Users;
using static App.Identity.Registration.Commands;

namespace App.Identity.Registration
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