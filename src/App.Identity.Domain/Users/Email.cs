using System;
using Eventuous;

namespace App.Identity.Domain.Users
{
    public record Email
    {
        public string Value { get; }
        public Email(string email) {
            if (String.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            Value = email;
        }

        public static implicit operator string(Email email) => email.Value;
    }
}
