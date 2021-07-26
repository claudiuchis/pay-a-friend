using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using Eventuous.Projections.MongoDB;

using static Pay.Prepaid.Projections.ReadModels;
using static Pay.Prepaid.Domain.PrepaidAccounts.Events;

namespace Pay.Prepaid.Projections
{
    public class PrepaidAccountProjector : MongoProjection<PrepaidAccount>
    {
        public PrepaidAccountProjector(
            IMongoDatabase database,
            string subscriptionGroup,
            ILoggerFactory loggerFactory
        ) : base(database, subscriptionGroup, loggerFactory) {}

        protected override ValueTask<Operation<PrepaidAccount>> GetUpdate(
            object @event,
            long? position
        )
        {
            return @event switch {
                V1.PrepaidAccountCreated created
                    => new(new CollectionOperation<PrepaidAccount>( (Collection, cancellationToken)
                        => Collection.InsertOneAsync(new PrepaidAccount(
                            created.PrepaidAccountId,
                            created.CustomerId,
                            created.CurrencyCode,
                            0.00,
                            System.Array.Empty<Transaction>()
                        )))),
                V1.PrepaidAccountCredited credited
                    => UpdateOperationTask(
                        credited.PrepaidAccountId,
                        u => u.Inc(d => d.Balance, (double) credited.Amount)
                            .AddToSet(d => d.Transactions, new Transaction { Credit = credited.Amount})
                    ),
                V1.PrepaidAccountDebited debited
                    => UpdateOperationTask(
                        debited.PrepaidAccountId,
                        u => u.Inc(d => d.Balance, (double) -debited.Amount)
                            .AddToSet(d => d.Transactions, new Transaction { Debit = debited.Amount})
                    ),
                _ => NoOp
            };
        }

    }
}