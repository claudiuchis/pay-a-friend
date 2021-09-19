using System.Threading.Tasks;
using EventStore.Client;
using Pay.Identity.Infrastructure;
using static Pay.Identity.Queries.ReadModels;

namespace Pay.Identity.Queries
{
    public class UserQueryService
    {
        EventStoreProjectionManagementClient _client;
        public UserQueryService(EventStoreProjectionManagementClient client)
        {
            _client = client;
        }

        // https://www.eventstore.com/blog/projections-5-indexing
        // https://developers.eventstore.com/clients/dotnet/5.0/projections/#the-number-of-items-per-shopping-cart
        // https://developers.eventstore.com/server/v21.6/docs/projections/user-defined-projections.html#user-defined-projections-api
        public async Task<UserDetails> GetUserByEmail(string email)
        {
            return await _client.GetStateAsync<UserDetails>(
                name: ProjectionMapping.UserDetailsProjection,
                partition: email);
        }
    }
}
