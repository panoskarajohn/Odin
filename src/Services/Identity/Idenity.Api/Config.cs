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
        }
    };

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>()
    {
        new ApiScope("eventApi", "Event Api")
    };

    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>();

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>();

    public static List<TestUser> TestUsers => new List<TestUser>();
}