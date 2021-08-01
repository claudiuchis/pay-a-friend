using System;

namespace Pay.WebApp.Configs
{
    public class IdentityProviderConfiguration
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SignUp { get; set; }
        public string SignIn { get; set; }
    }
}