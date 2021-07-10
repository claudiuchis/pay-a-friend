using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

using Eventuous.Projections.MongoDB;

using static Pay.TopUps.Projections.ReadModels;
using static Pay.TopUps.StripePayments.Events;

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
                                e.TransactionId, e.Amount, e.CurrencyCode, e.CustomerId, e.PaymentMethod, e.CardLast4Digits), null, cancellationToken))),
                _ => NoOp
            };
        }
    }
}