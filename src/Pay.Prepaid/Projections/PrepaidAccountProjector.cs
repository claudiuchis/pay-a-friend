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
                    => UpdateOperationTask(
                        created.PrepaidAccountId, 
                        u => u.Set(d => d.CustomerId, created.CustomerId)
                            .Set(d => d.CurrencyCode, created.CurrencyCode)
                            .Set(d => d.Balance, 0)
                            .Set(d => d.Transactions, System.Array.Empty<Transaction>())
                    ),
                V1.PrepaidAccountCredited credited
                    => UpdateOperationTask(
                        credited.PrepaidAccountId,
                        u => u.Inc(d => d.Balance, credited.Amount)
                            .AddToSet(d => d.Transactions, new Transaction { Credit = credited.Amount})
                    ),
                V1.PrepaidAccountDebited debited
                    => UpdateOperationTask(
                        debited.PrepaidAccountId,
                        u => u.Inc(d => d.Balance, -debited.Amount)
                            .AddToSet(d => d.Transactions, new Transaction { Debit = debited.Amount})
                    ),
                _ => NoOp
            };
        }

    }
}