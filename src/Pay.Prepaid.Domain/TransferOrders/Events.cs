namespace Pay.Prepaid.Domain.TransferOrders
{
    public static class Events
    {
        public static class V1
        {
            public record TransferOrderCreated
            (
                string TransferOrderId,
                string PayorPrepaidAccountId,
                string PayeePrepaidAccountId,
                string CurrencyCode,
                decimal Amount
            );
            public record TransferOrderCompleted
            (
                string TransferOrderId
            );

            public record TransferOrderFailed
            (
                string TransferOrderId,
                string Stage,
                string Reason
            );
        }
    }
}