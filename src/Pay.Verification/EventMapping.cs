using System;
using Eventuous;
using static Pay.Verification.Domain.Events;

namespace Pay.Verification
{
    internal static class EventMapping
    {
        public static void MapEventTypes()
        {
            TypeMap.AddType<V1.CustomerStartedVerification>("CustomerStartedVerification");
            TypeMap.AddType<V1.AddressAdded>("AddressAdded");
            TypeMap.AddType<V1.DateOfBirthAdded>("DateOfBirthAdded");
        }
    }
}