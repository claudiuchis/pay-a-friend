using System;
namespace Pay.Prepaid.Domain
{
    public record CustomerId
    {
        public string Value { get; init; }
        public CustomerId(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("CustomerId can't be empty");
            Value = value;
        }

        public static implicit operator string(CustomerId self) => self.Value;
    }
}