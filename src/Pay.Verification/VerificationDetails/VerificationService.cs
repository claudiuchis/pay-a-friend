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

            OnExisting<V1.AddDateOfBirth>(
                cmd => new VerificationDetailsId(cmd.VerificationDetailsId),
                (details, cmd) => details.AddDateOfBirth(DateOfBirth.FromString(cmd.DateOfBirth))
            );

            OnExisting<V1.AddAddress>(
                cmd => new VerificationDetailsId(cmd.VerificationDetailsId),
                (details, cmd) => details.AddAddress(new Address(cmd.Address1, cmd.Address2, cmd.CityTown, cmd.CountyState, cmd.Code, cmd.Country))
            );

            OnExisting<V1.SubmitDetails>(
                cmd => new VerificationDetailsId(cmd.VerificationDetailsId),
                (details, cmd) => details.SubmitDetails()
            );

        }
    }
}