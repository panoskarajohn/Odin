using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Idenity.Api;

public class Config
{
    public static IEnumerable<Client> Clients => new List<Client>()
    {
        new Client()
        {
            ClientId = "eventClient",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            AllowedScopes = { "eventApi" }
        },
        new Client()
        {
            ClientId = "backoffice",
            ClientName = "Backoffice",
            AllowedGrantTypes = GrantTypes.Code, 
            AllowRememberConsent = true,
            RedirectUris = new List<string>()
            {
                "http://localhost:10000/signin-oidc"
            },
            PostLogoutRedirectUris = new List<string>()
            {
                "http://localhost:10000/signout-callback-oidc"
            },
            ClientSecrets = new List<Secret>()
            {
                new Secret("secret".Sha256())
            },
            AllowedScopes = new List<string>()
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
            },
            RequirePkce = false
        }
    };

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>()
    {
        new ApiScope("eventApi", "Event Api")
    };

    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>();

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>()
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    };

    public static List<TestUser> TestUsers => new List<TestUser>()
    {
        new TestUser()
        {
            SubjectId = Guid.NewGuid().ToString("N"),
            Username = "panos",
            Password = "panos",
            Claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.GivenName, "panos"),
                new Claim(JwtClaimTypes.FamilyName, "karagiannis"),
                
            },
            IsActive = true

        }
    };
}