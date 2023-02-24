using System.Net;
using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Web.Options;

namespace Shared.Web.ServiceDiscovery;

public class ConsulRegistrationService : IHostedService
{

    private readonly IConsulClient _client;
    private readonly IServiceDiscoveryRegistration _serviceDiscoveryRegistration;
    private readonly ILogger<ConsulRegistrationService> _logger;
    private readonly string _serviceId;
    private readonly Uri _serviceUrl;
    private readonly string _serviceName;
    private readonly ConsulOptions _options;

    public ConsulRegistrationService(IConsulClient client, 
        IServiceDiscoveryRegistration serviceDiscoveryRegistration, 
        ILogger<ConsulRegistrationService> logger,  
        IOptions<ConsulOptions> options)
    {
        if (string.IsNullOrWhiteSpace(options.Value.Service.Name))
        {
            throw new ArgumentException("Service name is required.", nameof(options.Value.Service.Name));
        }
        
        if (string.IsNullOrWhiteSpace(options.Value.Service.Url))
        {
            throw new ArgumentException("Service url is required.", nameof(options.Value.Service.Url));
        }
        
        _client = client;
        _serviceDiscoveryRegistration = serviceDiscoveryRegistration;
        _logger = logger;
        _options = options.Value;
        _serviceUrl = new Uri(options.Value.Service.Url);
        _serviceName = options.Value.Service.Name;
        _serviceId  = $"{_serviceName}-{Guid.NewGuid():N}";
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Registering a service: '{_serviceId}' in Consul...");
        var result = await _client.Agent.ServiceRegister(new AgentServiceRegistration
        {
            ID = _serviceId,
            Name = _serviceName,
            Address = _serviceUrl.Host,
            Port = _serviceUrl.Port,
            Tags = _serviceDiscoveryRegistration.Tags.ToArray(),
            Check = new AgentServiceCheck
            {
                HTTP = $"{_serviceUrl}{_options.HealthCheck.Endpoint}",
                Interval = _options.HealthCheck.Interval,
                DeregisterCriticalServiceAfter = _options.HealthCheck.DeregisterInterval
            }
        }, cancellationToken);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation($"Registered a service: '{_serviceId}' in Consul.");
            return;
        }
        
        _logger.LogError($"There was an error: {result.StatusCode} when registering a service: '{_serviceId}' in Consul.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deregistering a service: '{_serviceId}' in Consul...");
        var result = await _client.Agent.ServiceDeregister(_serviceId, cancellationToken);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation($"Deregistered a service: '{_serviceId}' in Consul.");
            return;
        }
        
        _logger.LogError($"There was an error: {result.StatusCode} when deregistering a service: '{_serviceId}' in Consul.");
    }
}