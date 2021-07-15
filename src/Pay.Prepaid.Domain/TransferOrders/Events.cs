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

            public record PayorAccountDebited
            (
                string TransferOrderId
            );

            public record PayeeAccountCredited
            (
                string TransferOrderId
            );

            public record OrderFailed
            (
                string TransferOrderId,
                string Reason
            );
        }
    }
}