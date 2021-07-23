using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using Eventuous.Projections.MongoDB;

using static Pay.Prepaid.Projections.ReadModels;
using static Pay.Prepaid.Domain.TransferOrders.Events;

namespace Pay.Prepaid.Projections
{
    public class TransferOrderProjector : MongoProjection<TransferOrder>
    {
        public TransferOrderProjector(
            IMongoDatabase database,
            string subscriptionGroup,
            ILoggerFactory loggerFactory
        ) : base(database, subscriptionGroup, loggerFactory) {}

        protected override ValueTask<Operation<TransferOrder>> GetUpdate(
            object @event,
            long? position
        )
        {
            return @event switch {
                V1.TransferOrderCreated created
                    => UpdateOperationTask(
                        created.TransferOrderId, 
                        u => u.Set(d => d.PayorPrepaidAccountId, created.PayorPrepaidAccountId)
                            .Set(d => d.PayeePrepaidAccountId, created.PayeePrepaidAccountId)
                            .Set(d => d.Amount, created.Amount)
                            .Set(d => d.CurrencyCode, created.CurrencyCode)
                            .Set(d => d.Status, TransferOrderStatus.NotStarted)
                    ),
                V1.TransferOrderCompleted completed
                    => UpdateOperationTask(
                        completed.TransferOrderId,
                        u => u.Set(d => d.Status, TransferOrderStatus.OrderCompleted)
                    ),
                V1.TransferOrderFailed failed
                    => UpdateOperationTask(
                        failed.TransferOrderId,
                        u => u.Set(d => d.Status, TransferOrderStatus.OrderFailed)
                            .Set(d => d.Reason, failed.Reason)
                            .Set(d => d.Stage, failed.Stage)
                    ),
                _ => NoOp
            };
        }

    }

    public record TransferOrderStatus
    {
        public string Value { get; init; }
        TransferOrderStatus(string value) => Value = value;
        public static TransferOrderStatus NotStarted { get { return new TransferOrderStatus("NotStarted"); }}
        public static TransferOrderStatus OrderPlaced { get { return new TransferOrderStatus("OrderPlaced"); }}
        public static TransferOrderStatus OrderCompleted { get { return new TransferOrderStatus("OrderCompleted"); }}
        public static TransferOrderStatus OrderFailed { get { return new TransferOrderStatus("OrderFailed"); }}
        public static implicit operator string(TransferOrderStatus self) => self.Value;
    }

}