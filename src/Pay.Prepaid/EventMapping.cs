using Eventuous;
using Pay.Prepaid.Reactors;
using Pay.Prepaid.Domain.PrepaidAccounts;
using static Pay.Prepaid.Domain.TransferOrders.Events;

namespace Pay.Prepaid
{
    internal static class EventMapping
    {
        public static void MapEventTypes()
        {
            TypeMap.AddType<IntegrationEvents.V1.CustomerDetailsVerified>("CustomerDetailsVerified");
            TypeMap.AddType<IntegrationEvents.V1.TopUpCompleted>("TopUpCompleted");
            TypeMap.AddType<Events.V1.PrepaidAccountCreated>("PrepaidAccountCreated");
            TypeMap.AddType<Events.V1.PrepaidAccountCredited>("PrepaidAccountCredited");
            TypeMap.AddType<Events.V1.PrepaidAccountDebited>("PrepaidAccountDebited");
            TypeMap.AddType<Events.V1.PrepaidAccountHoldPlaced>("PrepaidAccountHoldPlaced");
            TypeMap.AddType<Events.V1.PrepaidAccountHoldReleased>("PrepaidAccountHoldReleased");
            TypeMap.AddType<V1.TransferOrderCreated>("TransferOrderCreated");
            TypeMap.AddType<V1.TransferOrderCompleted>("TransferOrderCompleted");
            TypeMap.AddType<V1.TransferOrderFailed>("TransferOrderFailed");
        }
    }
}