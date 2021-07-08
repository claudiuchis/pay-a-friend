using System;
using Eventuous;
using static Pay.TopUps.Domain.Events;

namespace Pay.TopUps
{
    internal static class EventMapping
    {
        public static void MapEventTypes()
        {
            TypeMap.AddType<V1.TopUpCompleted>("TopUpCompleted");
            TypeMap.AddType<V1.TopUpFailed>("TopUpFailed");
        }
    }
}