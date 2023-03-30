using WebApiGateway.Config;
using WebApiGateway.Versions;
using Yarp.ReverseProxy.Configuration;

namespace WebApiGateway;

public class ClusterConfiguration
{
    private readonly UrlsConfig _urlsConfig;

    public ClusterConfiguration(UrlsConfig urlsConfig)
    {
        _urlsConfig = urlsConfig;
    }

    public ClusterConfig[] Clusters => new[]
    {
        new ClusterConfig
        {
            ClusterId = ApiInfo.EventApi.EventClusterId,
            Destinations = new Dictionary<string, DestinationConfig>
            {
                {
                    ApiInfo.EventApi.EventClusterId, new DestinationConfig
                    {
                        Address = _urlsConfig.EventService
                    }
                }
            }
        },
        new ClusterConfig
        {
            ClusterId = ApiInfo.IdentityApi.IdentityClusterId,
            Destinations = new Dictionary<string, DestinationConfig>
            {
                {
                    ApiInfo.IdentityApi.IdentityClusterId, new DestinationConfig
                    {
                        Address = _urlsConfig.IdentityService
                    }
                }
            }
        },
        new ClusterConfig
        {
            ClusterId = ApiInfo.SlipApi.SlipClusterId,
            Destinations = new Dictionary<string, DestinationConfig>
            {
                {
                    ApiInfo.SlipApi.SlipClusterId, new DestinationConfig
                    {
                        Address = _urlsConfig.SlipService
                    }
                }
            }
        }
    };
}