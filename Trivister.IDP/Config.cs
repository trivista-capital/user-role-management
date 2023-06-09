using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Trivister.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "System roles", new[] { "role" }),
            new IdentityResource("permissions", "The permissions the user has", new List<string>() { "permission" })
        };
    
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("travisterApi", "Travister Api", new[]{ "role", "permission" })
            {
                Scopes = {"travisterApi.FullAccess", "travisterApi.Read", "travisterApi.Write"}
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("travisterApi.FullAccess"),
            new ApiScope("travisterApi.Read"),
            new ApiScope("travisterApi.Write")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client()
            {
                ClientName = "UserManagementApi",
                ClientId = "UserManagementApi",
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris =
                {
                    "https://localhost:7262/signin-oidc"
                },
                PostLogoutRedirectUris =
                {
                        "https://localhost:7262/signout-callback-oidc"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles",
                    "permissions",
                    "travisterApi.FullAccess"
                },
                ClientSecrets=
                {
                    new Secret("secret".Sha256())
                }
            }
        };
}