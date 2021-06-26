using System;
using Eventuous;
using static Pay.Identity.Domain.Users.Events;

namespace Pay.Identity
{
    internal static class EventMapping
    {
        public static void MapEventTypes()
        {
            TypeMap.AddType<V1.UserRegistered>("UserRegistered");
        }
    }
}