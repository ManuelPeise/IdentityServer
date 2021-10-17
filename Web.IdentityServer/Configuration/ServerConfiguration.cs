using IdentityServer4.Models;
using System.Collections.Generic;

namespace Web.IdentityServer.Configuration
{
    public class ServerConfiguration
    {
        public static List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "identityServerUiClient",
                    ClientName = "Identity Server Ui Client",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("password".Sha256())},
                    AllowedScopes = new List<string> {"ui.show", "ui.administration"}

                }
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Address(),
                new IdentityResource
                {
                    Name = "userRole",
                    UserClaims = new List<string> { "userRole" }
                }
            };
        }

        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "identityServerUi",
                    DisplayName = "Identity Server Administration Ui",
                    Description = "Allow to access the Identity Server Administration Ui",
                    Scopes = new List<string> {"ui.show", "ui.administration"},
                    ApiSecrets = new List<Secret> {new Secret("password".Sha256())}, // get from config file
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("ui.show", "Allow to access the Identity Server Ui."),
                new ApiScope("ui.administration", "Allow to administrate the Identit Server.")
            };
        }
    }
}
