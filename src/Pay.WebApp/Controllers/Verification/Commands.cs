namespace Pay.WebApp
{
    public static class Commands
    {
        public class CreateDraftVerificationDetails
        {
            public string VerificationDetailsId { get; set; }
            public string CustomerId { get; set; }
        }
    }
}