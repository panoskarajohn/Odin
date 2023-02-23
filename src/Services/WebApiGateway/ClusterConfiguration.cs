using Yarp.ReverseProxy.Configuration;

namespace WebApiGateway;

public static class ClusterConfiguration
{
    public const string EventClusterId = "event";
    public const string IdentityClusterId = "identity";
    public const string SlipClusterId = "slip";

    public static ClusterConfig[] Clusters => new[]
    {
        new ClusterConfig()
        {
            ClusterId = EventClusterId,
            Destinations = new Dictionary<string, DestinationConfig>()
            {
                {
                    EventClusterId, new DestinationConfig()
                    {
                        Address = "http://localhost:2000"
                    }
                }
            }
        },
        new ClusterConfig()
        {
            ClusterId = IdentityClusterId,
            Destinations = new Dictionary<string, DestinationConfig>()
            {
                {
                    IdentityClusterId, new DestinationConfig()
                    {
                        Address = "http://localhost:3000"
                    }
                }
            }
        },
        new ClusterConfig()
        {
            ClusterId = SlipClusterId,
            Destinations = new Dictionary<string, DestinationConfig>()
            {
                {
                    SlipClusterId, new DestinationConfig()
                    {
                        Address = "http://localhost:5000"
                    }
                }
            }
        }
    };
}