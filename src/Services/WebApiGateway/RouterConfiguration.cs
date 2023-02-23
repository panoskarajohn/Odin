using WebApiGateway.Versions;
using Yarp.ReverseProxy.Configuration;

namespace WebApiGateway;

public static class RouterConfiguration
{
    const string CatchAll = "{**catch-all}";
    const string EventPathTransform = $"/api/{EventApi.V1}/match/{CatchAll}";
    public static RouteConfig[] Routes => new []
    {
        new RouteConfig()
        {
            RouteId = ClusterConfiguration.EventClusterId,
            ClusterId = ClusterConfiguration.EventClusterId,
            Match = new RouteMatch()
            {
                Path = "event/{**catch-all}",
                Methods = new[] {"GET", "POST", "PUT", "DELETE"},
            },
            Transforms = new[]
            {
                new Dictionary<string, string>()
                {
                    {"PathPattern", EventPathTransform}
                }
            },
            AuthorizationPolicy = "jwt"
        }
    };
}