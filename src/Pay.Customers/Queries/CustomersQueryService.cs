using System.Threading.Tasks;
using MongoDB.Driver;
using Pay.Customers.Projections;

namespace Pay.Customers.Queries
{
    public class CustomersQueryService
    {
        IMongoCollection<CustomersDetails> _database;
        public CustomersQueryService(
            IMongoDatabase database
        )
        {
            _database = database.GetCollection<CustomersDetails>(typeof(CustomersDetails).Name);
        }

        public async Task<CustomersDetails> GetCustomerById(string customerId)
        {
            var query = await _database.FindAsync(d => d.Id == customerId);
            return await query.SingleOrDefaultAsync();
        }
    }
}