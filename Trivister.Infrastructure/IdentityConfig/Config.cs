using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Trivister.Infrastructure.IdentityConfig;

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
    
    //This points to our API. The api are scopes
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("travisterApi", "Travister Api", new[]{ "role", "permission" })
            {
                Scopes = {"travisterApi.FullAccess", "travisterApi.Read", "travisterApi.Write", "fullAccess"}
            },
            new ApiResource("travisterApiLoanApi", "TravisterApi LoanApi", new[]{ "role", "permission" })
            {
                Scopes = {"travisterApiLoanApi.FullAccess", "travisterApiLoanApi.Read", "travisterApiLoanApi.Write", "fullAccess"}
            }
        };
    //API can have api scopes
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("travisterApi.FullAccess"),
            new ApiScope("travisterApi.Read"),
            new ApiScope("travisterApi.Write"),
            new ApiScope("travisterApiLoanApi.FullAccess"),
            new ApiScope("travisterApiLoanApi.Read"),
            new ApiScope("travisterApiLoanApi.Write"),
            new ApiScope("fullAccess")
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
                    "travisterApi.FullAccess",
                    "fullAccess"
                },
                ClientSecrets=
                {
                    new Secret("secret".Sha256())
                }
            },
            new Client()
            {
                ClientName = "LoanManagementApi",
                ClientId = "LoanManagementApi",
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
                    "travisterApiLoanApi.FullAccess",
                    "fullAccess"
                },
                ClientSecrets=
                {
                    new Secret("secret".Sha256())
                }
            },
            new Client()
            {
                ClientName = "Mobile",
                ClientId = "MobileClient",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowOfflineAccess = true,
                RequireConsent = false,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles",
                    "permissions",
                    "travisterApi.FullAccess",
                    "travisterApiLoanApi.FullAccess",
                    "fullAccess"
                },
                ClientSecrets=
                {
                    new Secret("secret".Sha256())
                }
            }
        };
}