namespace Shared.Web.ServiceDiscovery;

public interface IServiceDiscoveryRegistration
{
    IEnumerable<string> Tags { get; }
}