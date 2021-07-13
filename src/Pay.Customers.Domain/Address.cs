using System;

namespace Pay.Verification.Domain
{
    public class Address
    {
        public string Address1 { get; }
        public string Address2 { get; }
        public string CityTown { get; }
        public string CountyState { get; }
        public string Code { get; }
        public string Country { get; }
        public string CountryCode { get; }

        public Address(
            string address1, 
            string address2, 
            string cityTown, 
            string countyState, 
            string code, 
            string country,
            string countryCode
        )
        {
            if (string.IsNullOrWhiteSpace(address1))
                throw new ArgumentNullException(nameof(address1));
            if (string.IsNullOrWhiteSpace(cityTown))
                throw new ArgumentNullException(nameof(cityTown));
            if (string.IsNullOrWhiteSpace(countyState))
                throw new ArgumentNullException(nameof(countyState));
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentNullException(nameof(country));
            if (string.IsNullOrWhiteSpace(countryCode))
                throw new ArgumentNullException(nameof(countryCode));

            Address1 = address1;
            Address2 = address2;
            CityTown = cityTown;
            CountyState = countyState;
            Code = code;
            Country = country;
            CountryCode = countryCode;
        }
    }
}