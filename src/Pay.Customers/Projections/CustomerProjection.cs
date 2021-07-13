using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

using Eventuous.Projections.MongoDB;
using static Pay.Verification.Projections.ReadModels;
using static Pay.Verification.Domain.Events;

namespace Pay.Verification.Projections
{
    public class CustomerProjection : MongoProjection<Customer>
    {
        public CustomerProjection(IMongoDatabase database, string subscriptionGroup, ILoggerFactory loggerFactory)
            : base(database, subscriptionGroup, loggerFactory) {}

        protected override ValueTask<Operation<Customer>> GetUpdate(object @event, long? position)
        {
            return @event switch{
                V1.CustomerCreated e 
                    => new(new CollectionOperation<Customer>( (collection, cancellationToken) 
                        => collection.InsertOneAsync(new Customer(e.CustomerId, VerificationStatus.DetailsNotVerified), null, cancellationToken))),
                V1.AddressAdded e 
                    => UpdateOperationTask(e.CustomerId, update 
                        => update.Set(d => d.Address, $"{e.Address1} {e.Address2}, {e.CityTown}, {e.CountyState}, {e.Code}, {e.Country}")),
                V1.DateOfBirthAdded e
                    => UpdateOperationTask(e.CustomerId, update
                        => update.Set(d => d.DateOfBirth, e.DateOfBirth)),
                V1.CustomerDetailsSentForVerification e
                    => UpdateOperationTask(e.CustomerId, update
                        => update.Set(d => d.VerificationStatus, VerificationStatus.DetailsSentForVerification)),
                V1.CustomerDetailsVerified e
                    => UpdateOperationTask(e.CustomerId, update
                        => update.Set(d => d.VerificationStatus, VerificationStatus.DetailsVerified)),
                _ => NoOp
            };
        }
    }
}