using Eventuous;
using Pay.Prepaid.Reactors;
using Pay.Prepaid.Domain.PrepaidAccounts;

namespace Pay.Prepaid
{
    internal static class EventMapping
    {
        public static void MapEventTypes()
        {
            TypeMap.AddType<IntegrationEvents.V1.CustomerVerified>("CustomerVerified");
            TypeMap.AddType<IntegrationEvents.V1.TopUpCompleted>("TopUpCompleted");
            TypeMap.AddType<Events.V1.PrepaidAccountCreated>("PrepaidAccountCreated");
            TypeMap.AddType<Events.V1.PrepaidAccountCredited>("PrepaidAccountCredited");
            TypeMap.AddType<Events.V1.PrepaidAccountDebited>("PrepaidAccountDebited");
        }
    }
}