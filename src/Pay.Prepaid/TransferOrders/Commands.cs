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

            public record AcknowlegePayorAccountDebited(
                string TransferOrderId
            );

            public record AcknowledgePayeeAccountCredited(
                string TransferOrderId
            );

            public record AcknowledgeOrderFailed(
                string TransferOrderId,
                string Reason
            );
        }
    }
}