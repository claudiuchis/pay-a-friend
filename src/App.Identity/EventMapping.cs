using System;
using Eventuous;
using static App.Identity.Domain.Users.Events;

namespace App.Identity
{
    internal static class EventMapping
    {
        public static void MapEventTypes()
        {
            TypeMap.AddType<V1.UserRegistered>("UserRegistered");
        }
    }
}