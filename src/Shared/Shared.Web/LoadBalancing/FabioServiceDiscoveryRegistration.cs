using Microsoft.Extensions.Options;
using Shared.Web.Options;
using Shared.Web.ServiceDiscovery;

namespace Shared.Web.LoadBalancing;

internal sealed class FabioServiceDiscoveryRegistration : IServiceDiscoveryRegistration
{
    public FabioServiceDiscoveryRegistration(IOptions<ConsulOptions> options)
    {
        var serviceName = options.Value.Service.Name;
        Tags = new[] {$"urlprefix-/{serviceName} strip=/{serviceName}"};
    }

    public IEnumerable<string> Tags { get; }
}