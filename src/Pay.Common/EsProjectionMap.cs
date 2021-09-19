using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventStore.Client;
using System.Linq;

namespace Pay.Common
{
    public record Projection(string Name, long Version, string Query){};
    public static class EsProjectionMap
    {
        
        static readonly List<Projection> Projections = new();
        public static void AddProjection(Projection projection)
            => Projections.Add(projection);

        public static async Task UpsertProjections(EventStoreProjectionManagementClient client)
        {
            foreach (var projection in Projections)
            {
                try {
                    var details = await client.GetStatusAsync(projection.Name);
                    if (details.Version < projection.Version)
                    {
                        await client.UpdateAsync(projection.Name, projection.Query);
                    }
                }
                catch(InvalidOperationException)
                {
                    await client.CreateContinuousAsync(projection.Name, projection.Query, true);
                }
            }
        }
    }
}