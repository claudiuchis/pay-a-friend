using System;
using Eventuous;
using Pay.Verification.Domain;
using static Pay.Verification.Commands;

namespace Pay.Verification
{
    public class VerificationCommandService : ApplicationService<VerificationRequest, VerificationRequestState, CustomerId>
    {
        public VerificationCommandService(
            IAggregateStore store
        ) : base(store)
        {
            OnAny<V1.SubmitVerification>(
                cmd => new CustomerId(cmd.CustomerId),
                (request, cmd)
                    => request.SubmitVerification(
                        new CustomerId(cmd.CustomerId),
                        new Name(cmd.FirstName, cmd.LastName),
                        DateOfBirth.FromString(cmd.DateOfBirth),
                        new Address(cmd.Address1, cmd.Address2, cmd.CityTown, cmd.CountyState, cmd.Code, cmd.Country, cmd.CountryCode),
                        cmd.ProofOfIdentity,
                        cmd.ProofOfAddress
                    )
            );

            OnExisting<V1.CompleteVerification>(
                cmd => new CustomerId(cmd.CustomerId),
                (request, cmd) => request.CompleteVerification()
            );

        }
    }
}