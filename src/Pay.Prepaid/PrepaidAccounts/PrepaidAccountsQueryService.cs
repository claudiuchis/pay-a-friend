using MongoDB.Driver;
using Eventuous.Projections.MongoDB.Tools;

using static Pay.Prepaid.Projections.ReadModels;

namespace Pay.Prepaid.PrepaidAccounts
{
    public class PrepaidAccountsQueryService
    {
        IMongoDatabase _database;
        public PrepaidAccountsQueryService(
            IMongoDatabase database
        )
        {
            _database = database;
        }

        public string GetPrepaidAccountForCustomer(string customerId)
        {
            var collection = _database.GetDocumentCollection<PrepaidAccount>();
            var filter = Builders<PrepaidAccount>.Filter.Eq(d => d.CustomerId, customerId);
            var doc = collection.Find(filter).FirstOrDefault();
            return doc.PrepaidAccountId;
        }
    }
}