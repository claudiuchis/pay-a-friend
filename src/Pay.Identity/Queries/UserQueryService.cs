using System.Threading.Tasks;
using EventStore.Client;
using Pay.Identity.Infrastructure;

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
        public async Task<UserDetails> GetUser(string UserId)
        {
            return await GetStateAsync<UserDetails>(
                name: ProjectionMapping.UserDetailsProjection,
                partitionId: $"User-{UserId}");
        }
    }
}
