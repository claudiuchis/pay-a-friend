
namespace Pay.TopUps.Domain
{
    public record CardDetails()
    {
        public string Name { get; init; }
        public string Number { get; init; }
        public string ExpMonth { get; init; }
        public string ExpYear { get; init; }
        public string Cvc { get; init; }
        CardDetails() {}
        
        public CardDetails(
            string name, 
            string number,
            string expMonth, 
            string expYear,
            string cvc)
        {
            if (cvc.Length != 3)
                throw new ArgumentException($"Cvc must be 3 digits");
            
            if (number.Length != 16)
                throw new ArgumentException($"The credit card number must have 16 digits");

            if (expMonth.Length != 2)
                throw new ArgumentException($"The month must in the xx format (e.g. 01 for the month of January)");

            if (expYear.Length != 2)
                throw new ArgumentException($"The year must in the xx format (e.g. 21 for year 2021)");

            Name = name;
            Number = number;
            ExpMonth = expMonth;
            ExpYear = expYear;
            Cvc = cvc;
        }

    }
}