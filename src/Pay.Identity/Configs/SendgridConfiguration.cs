// dotnet user-secrets set "Section:Key" "Value

namespace Pay.Identity.Configs
{
    public class SendgridConfiguration
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set;}
        public string SenderName { get; set; }
    }
}