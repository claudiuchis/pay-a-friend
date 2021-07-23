using System.Threading.Tasks;
using MongoDB.Driver;
using Eventuous.Projections.MongoDB.Tools;

using static Pay.Prepaid.Projections.ReadModels;

namespace Pay.Prepaid.TransferOrders
{
    public class TransferOrdersQueryService
    {
        IMongoDatabase _database;
        public TransferOrdersQueryService(
            IMongoDatabase database
        )
        {
            _database = database;
        }

        public async Task<TransferOrder> GetTransferOrder(string transferOrderId)
        {
            var collection = _database.GetDocumentCollection<TransferOrder>();
            var filter = Builders<TransferOrder>.Filter.Eq(d => d.TransferOrderId, transferOrderId);
            var result = await collection.FindAsync(filter);
            return result.FirstOrDefault();
        }
    }
}