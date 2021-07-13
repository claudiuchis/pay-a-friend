using System;
using Eventuous;
using static Pay.Verification.Domain.Events;

namespace Pay.Verification.Domain
{
    public class Customer : Aggregate<CustomerState, CustomerId>
    {
        public void CreateCustomer(CustomerId customerId) {
            EnsureDoesntExist();
            Apply(new V1.CustomerCreated(customerId));
        }

        public void AddDateOfBirth(DateOfBirth dob) {
            Apply(new V1.DateOfBirthAdded(GetId(), dob));
        }

        public void AddAddress(Address address) {
            Apply(new V1.AddressAdded(
                GetId(), 
                address.Address1, 
                address.Address2, 
                address.CityTown, 
                address.CountyState, 
                address.Code, 
                address.Country,
                address.CountryCode
            ));
        }

        public void SubmitDetailsForVerification() {
            Apply(new V1.CustomerDetailsSentForVerification(GetId()));
        }

        public void VerifyCustomerDetails() {
            Apply(new V1.CustomerDetailsVerified(
                GetId(),
                State.Address.CountryCode
            ));
        }
    }

    public record CustomerState : AggregateState<CustomerState, CustomerId>
    {
        public enum CustomerDetailsVerificationStatus {
            CustomerDetailsSentForVerification,
            CustomerDetailsPartiallyVerified,
            CustomerDetailsVerified
        }
        public DateOfBirth DateOfBirth { get; init; }
        public Address Address { get; init; }
        public CustomerDetailsVerificationStatus VerificationStatus { get; init; }

        public override CustomerState When(object @event)
            => @event switch {
                V1.CustomerCreated created => this with {
                    Id = new CustomerId(created.CustomerId),
                },
                V1.DateOfBirthAdded dob => this with {
                    DateOfBirth = DateOfBirth.FromString(dob.DateOfBirth)
                },
                V1.AddressAdded address => this with {
                    Address = new Address(
                        address.Address1, 
                        address.Address2, 
                        address.CityTown, 
                        address.CountyState, 
                        address.Code, 
                        address.Country,
                        address.CountryCode)
                },
                V1.CustomerDetailsSentForVerification sent => this with {
                    VerificationStatus = CustomerDetailsVerificationStatus.CustomerDetailsSentForVerification
                },
                V1.CustomerDetailsVerified verified => this with {
                    VerificationStatus = CustomerDetailsVerificationStatus.CustomerDetailsVerified
                },
                _ => this
            };

    }
}
