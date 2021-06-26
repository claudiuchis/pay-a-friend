using System;
using Eventuous;

namespace Pay.Identity.Domain.Users
{
    public record FullName
    {
        public string Value { get; }
        public FullName(string fullName) {
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentNullException(nameof(fullName));
            Value = fullName;
        }

        public static implicit operator string(FullName fullName) => fullName.Value;
    }
}
