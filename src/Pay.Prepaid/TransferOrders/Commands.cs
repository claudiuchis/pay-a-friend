namespace Pay.Prepaid.TransferOrders
{
    public static class Commands
    {
        public static class V1
        {
            public record CreateTransferOrder(
                string TransferOrderId,
                string PayorPrepaidAccountId,
                string PayeePrepaidAccountId,
                decimal Amount,
                string CurrencyCode
            );

            public record CompleteTransferOrder(
                string TransferOrderId
            );

            public record FailTranferOrder(
                string TransferOrderId,
                string Stage,
                string Reason
            );
        }
    }
}