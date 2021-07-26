using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Pay.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("pay.customers", "Pay.Customers")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "pay.webapp",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:6001/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "pay.customers"
                    }
                }
            };
    }
}