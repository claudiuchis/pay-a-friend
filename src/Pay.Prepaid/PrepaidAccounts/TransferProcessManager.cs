namespace Pay.Prepaid.PrepaidAccounts
{
    public class TransferProcessManager
    {
        public enum TransferProcessState
        {
            NotStarted,
            PayorAccountDebited,
            PayeeAccountCredited,
        }
    }
}