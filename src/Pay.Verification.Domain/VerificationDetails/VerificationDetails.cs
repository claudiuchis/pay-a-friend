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
    }

    public record VerificationDetailsState : AggregateState<VerificationDetailsState, VerificationDetailsId>
    {
        CustomerId CustomerId { get; init; }

        public override VerificationDetailsState When(object @event)
            => @event switch {
                V1.CustomerStartedVerification started => this with {
                    Id = new VerificationDetailsId(started.VerificationDetailsId),
                    CustomerId = new CustomerId(started.CustomerId)
                },
                _ => this
            };

    }
}
