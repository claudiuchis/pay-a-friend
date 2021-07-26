using System;
using Eventuous;
using Pay.Verification.Domain;
using static Pay.Verification.Commands;

namespace Pay.Verification
{
    public class CustomersCommandService : ApplicationService<Customer, CustomerState, CustomerId>
    {
        public CustomersCommandService(
            IAggregateStore store
        ) : base(store)
        {
            OnAny<V1.CreateCustomer>(
                cmd => new CustomerId(cmd.CustomerId),
                (customer, cmd)
                    => customer.CreateCustomer(
                        new CustomerId(cmd.CustomerId)
                    )
            );

            OnExisting<V1.AddDateOfBirth>(
                cmd => new CustomerId(cmd.CustomerId),
                (customer, cmd) => customer.AddDateOfBirth(
                    DateOfBirth.FromString(cmd.DateOfBirth))
            );

            OnExisting<V1.AddAddress>(
                cmd => new CustomerId(cmd.CustomerId),
                (customer, cmd) => customer.AddAddress(
                    new Address(
                        cmd.Address1, 
                        cmd.Address2, 
                        cmd.CityTown, 
                        cmd.CountyState, 
                        cmd.Code, 
                        cmd.Country,
                        cmd.CountryCode))
            );

            OnExisting<V1.SubmitDetailsForVerification>(
                cmd => new CustomerId(cmd.CustomerId),
                (customer, cmd) => customer.SubmitDetailsForVerification()
            );

            OnExisting<V1.VerifyCustomerDetails>(
                cmd => new CustomerId(cmd.CustomerId),
                (customer, cmd) => customer.VerifyCustomerDetails()
            );

        }
    }
}