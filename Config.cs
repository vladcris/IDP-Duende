using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityProvider.Duende;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(),
            new IdentityResource("roles", "Your role(s)", new List<string> { "role" }),
            new IdentityResource("country", "Your country", new List<string> { "country" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("api.scope", "API")
            };
    public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api", "API") {
                    Scopes = { "api.scope" },
                    UserClaims = new List<string> { "role",}
                }
            };

    public static IEnumerable<Client> Clients =>
        new Client[] 
        {
            new Client {
                ClientName = "WebClient",
                ClientId = "webclient",
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:7045/signin-oidc" },
                AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, 
                                  IdentityServerConstants.StandardScopes.Profile,
                                  IdentityServerConstants.StandardScopes.Address,
                                  "roles",
                                  "api.scope",
                                  "country"},
                ClientSecrets = { new Secret("secret".Sha512()) },
                RequirePkce = true,
                RequireConsent = true,
                PostLogoutRedirectUris = new List<string> { "https://localhost:7045/signout-callback-oidc" },
                ClientUri = "https://localhost:7045",
                AccessTokenLifetime = 120,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,

            }
        };
}