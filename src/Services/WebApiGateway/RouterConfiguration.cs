using WebApiGateway.Versions;
using Yarp.ReverseProxy.Configuration;

namespace WebApiGateway;

public class RouterConfiguration
{
    private const string CatchAll = "{**catch-all}";

    public RouteConfig[] Routes => new[]
    {
        new RouteConfig
        {
            RouteId = ApiInfo.EventApi.EventClusterId,
            ClusterId = ApiInfo.EventApi.EventClusterId,
            Match = new RouteMatch
            {
                Path = "event/{**catch-all}",
                Methods = new[] {"GET", "POST", "PUT", "DELETE"}
            },
            Transforms = new[]
            {
                new Dictionary<string, string>
                {
                    {"PathPattern", ApiInfo.EventApi.EventV1PathTransform}
                }
            },
            AuthorizationPolicy = "jwt"
        },
        new RouteConfig
        {
            RouteId = ApiInfo.IdentityApi.IdentityClusterId,
            ClusterId = ApiInfo.IdentityApi.IdentityClusterId,
            Match = new RouteMatch
            {
                Path = "identity/{**catch-all}",
                Methods = new[] {"GET", "POST", "PUT", "DELETE"}
            },
            Transforms = new[]
            {
                new Dictionary<string, string>
                {
                    {"PathPattern", ApiInfo.IdentityApi.IdentityV1PathTransform}
                }
            },
            AuthorizationPolicy = "jwt"
        }
    };
}