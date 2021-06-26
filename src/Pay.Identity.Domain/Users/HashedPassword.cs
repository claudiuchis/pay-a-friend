using System;
using Eventuous;

namespace Pay.Identity.Domain.Users
{
    public record HashedPassword
    {
        internal HashedPassword(string HashedPassword) => Value = HashedPassword;
        public string Value { get; }
        public static HashedPassword FromString(
            string plainPassword,
            Func<string, string> hashPassword
        ) {
            if (String.IsNullOrEmpty(plainPassword))
                throw new ArgumentNullException(nameof(plainPassword));
            return new HashedPassword(hashPassword(plainPassword));
        }

        public static implicit operator string(HashedPassword userPassword) => userPassword.Value;
    }
}
