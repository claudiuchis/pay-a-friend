using System;

namespace Pay.Verification.Domain
{
    public class Name
    {
        public string FirstName { get; }
        public string LastName { get; }

        public Name(
            string firstName,
            string lastName
        )
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName));

            FirstName = firstName;
            LastName = lastName;
        }
    }
}