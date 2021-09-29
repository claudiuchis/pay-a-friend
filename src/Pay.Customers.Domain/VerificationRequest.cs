using Eventuous;
using static Pay.Verification.Domain.Events;

namespace Pay.Verification.Domain
{
    public class VerificationRequest : Aggregate<VerificationRequestState, CustomerId>
    {
        public void SubmitVerification(
            CustomerId customerId,
            Name name,
            DateOfBirth dateOfBirth,
            Address address,
            string proofOfIdentity,
            string proofOfAddress
            )
        {
            EnsureDoesntExist();
            Apply(new V1.VerificationSubmitted(
                customerId,
                name.FirstName,
                name.LastName,
                dateOfBirth,
                address.Address1,
                address.Address2,
                address.CityTown,
                address.CountyState,
                address.Code,
                address.Country,
                address.CountryCode,
                proofOfIdentity,
                proofOfAddress));
        }

        public void CompleteVerification()
        {
            Apply(new V1.VerificationCompleted(
                GetId()
            ));
        }
    }

    public record VerificationRequestState : AggregateState<VerificationRequestState, CustomerId>
    {
        public enum VerificationStatus { Submitted, Verified }
        public Name Name { get; set; }
        public DateOfBirth DateOfBirth { get; init; }
        public Address Address { get; init; }
        public VerificationStatus Status { get; init; }
        public string ProofOfIdentity { get; set; }
        public string ProofOfAddress { get; set; }

        public override VerificationRequestState When(object @event)
            => @event switch {
                V1.VerificationSubmitted submitted => this with {
                    Id = new CustomerId(submitted.CustomerId),
                    Name = new Name(submitted.FirstName, submitted.LastName),
                    DateOfBirth = DateOfBirth.FromString(submitted.DateOfBirth),
                    Address = new Address(
                        submitted.Address1, 
                        submitted.Address2, 
                        submitted.CityTown, 
                        submitted.CountyState, 
                        submitted.Code, 
                        submitted.Country,
                        submitted.CountryCode),
                    ProofOfIdentity = submitted.ProofOfIdentity,
                    ProofOfAddress = submitted.ProofOfAddress,
                    Status = VerificationStatus.Submitted
                },
                V1.VerificationCompleted completed => this with {
                    Status = VerificationStatus.Verified
                },
                _ => this
            };
    }
}