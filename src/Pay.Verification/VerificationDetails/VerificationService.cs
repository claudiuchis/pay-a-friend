using System;
using Eventuous;
using Pay.Verification.Domain;
using static Pay.Verification.Commands;

namespace Pay.Verification
{
    public class VerificationService : ApplicationService<VerificationDetails, VerificationDetailsState, VerificationDetailsId>
    {
        public VerificationService(
            IAggregateStore store
        ) : base(store)
        {
            OnNew<V1.CreateVerificationDetailsDraft>(
                cmd => new VerificationDetailsId(cmd.VerificationDetailsId),
                (verificationDetails, cmd)
                    => verificationDetails.CreateDraft(
                        new VerificationDetailsId(cmd.VerificationDetailsId),
                        new CustomerId(cmd.CustomerId)
                    )
            );
        }
    }
}