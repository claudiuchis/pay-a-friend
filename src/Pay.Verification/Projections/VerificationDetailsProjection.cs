using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

using Eventuous.Projections.MongoDB;
using static Pay.Verification.Projections.ReadModels;
using static Pay.Verification.Domain.Events;

namespace Pay.Verification.Projections
{
    public class VerificationDetailsProjection : MongoProjection<VerificationDetails>
    {
        public VerificationDetailsProjection(IMongoDatabase database, string subscriptionGroup, ILoggerFactory loggerFactory)
            : base(database, subscriptionGroup, loggerFactory) {}

        protected override ValueTask<Operation<VerificationDetails>> GetUpdate(object @event, long? position)
        {
            return @event switch{
                V1.CustomerStartedVerification e => new (new CollectionOperation<VerificationDetails>( (collection, cancellationToken) 
                    => collection.InsertOneAsync(new VerificationDetails(
                        e.VerificationDetailsId, e.CustomerId)))),
                _ => NoOp
            };
        }
    }
}