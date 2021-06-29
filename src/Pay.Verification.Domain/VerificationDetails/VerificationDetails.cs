using System;
using Eventuous;
using static Pay.Verification.Domain.Events;

namespace Pay.Verification.Domain
{
    public class VerificationDetails : Aggregate<VerificationDetailsState, VerificationDetailsId>
    {
        public void CreateDraft(VerificationDetailsId detailsId, CustomerId customerId) {
            EnsureDoesntExist();
            Apply(new V1.CustomerStartedVerification(detailsId, customerId));
        }

        public void AddDateOfBirth(DateOfBirth dob) {
            Apply(new V1.DateOfBirthAdded(GetId(), dob));
        }

        public void AddAddress(Address address) {
            Apply(new V1.AddressAdded(GetId(), address.Address1, address.Address2, address.CityTown, address.CountyState, address.Code, address.Country));
        }

        public void SubmitDetails() {
            Apply(new V1.DetailsSubmitted(GetId()));
        }
    }

    public record VerificationDetailsState : AggregateState<VerificationDetailsState, VerificationDetailsId>
    {
        public enum VerificationStatus {
            Draft = 0,
            Pending,
            Approved
        }
        CustomerId CustomerId { get; init; }
        DateOfBirth DateOfBirth { get; init; }
        Address Address { get; init; }
        VerificationStatus Status { get; init; }

        public override VerificationDetailsState When(object @event)
            => @event switch {
                V1.CustomerStartedVerification started => this with {
                    Id = new VerificationDetailsId(started.VerificationDetailsId),
                    CustomerId = new CustomerId(started.CustomerId),
                    Status = VerificationStatus.Draft
                },
                V1.DateOfBirthAdded dob => this with {
                    DateOfBirth = DateOfBirth.FromString(dob.DateOfBirth)
                },
                V1.AddressAdded address => this with {
                    Address = new Address(address.Address1, address.Address2, address.CityTown, address.CountyState, address.Code, address.Country)
                },
                V1.DetailsSubmitted submitted => this with {
                    Status = VerificationStatus.Pending
                },
                _ => this
            };

    }
}
