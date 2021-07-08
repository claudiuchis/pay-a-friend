using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

using Eventuous.Projections.MongoDB;

using static Pay.TopUps.Projections.ReadModels;
using static Pay.TopUps.Domain.Events;

namespace Pay.TopUps.Projections
{
    public class TopUpProjection : MongoProjection<TopUpDetails>
    {
        public TopUpProjection(IMongoDatabase database, string subscriptionGroup, ILoggerFactory loggerFactory)
            : base(database, subscriptionGroup, loggerFactory) {}

        protected override ValueTask<Operation<TopUpDetails>> GetUpdate(object @event, long? position)
        {
            return @event switch {
                V1.TopUpCompleted e
                    => new(new CollectionOperation<TopUpDetails>( (collection, cancellationToken) 
                        => collection.InsertOneAsync(new TopUpDetails(
                                e.TopUpId, e.Amount, e.CurrencyCode, e.PaymentId, TopUpOutcome.Success, null), null, cancellationToken))),
                V1.TopUpFailed e
                    => new(new CollectionOperation<TopUpDetails>( (collection, cancellationToken) 
                        => collection.InsertOneAsync(new TopUpDetails(
                                e.TopUpId, e.Amount, e.CurrencyCode, null, TopUpOutcome.Fail, e.Reason), null, cancellationToken))),
                _ => NoOp
            };
        }
    }
}