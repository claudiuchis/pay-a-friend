using Eventuous;
using static Pay.TopUps.StripePayments.Events;

namespace Pay.TopUps
{
    internal static class EventMapping
    {
        public static void MapEventTypes()
        {
            TypeMap.AddType<V1.TopUpCompleted>("TopUpCompleted");
        }
    }
}