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
    private readonly ILogger<ConsulRegistrationService> _logger;
    private readonly ConsulOptions _options;
    private readonly IServiceDiscoveryRegistration _serviceDiscoveryRegistration;
    private readonly string _serviceId;
    private readonly string _serviceName;
    private readonly Uri _serviceUrl;

    public ConsulRegistrationService(IConsulClient client,
        IServiceDiscoveryRegistration serviceDiscoveryRegistration,
        ILogger<ConsulRegistrationService> logger,
        IOptions<ConsulOptions> options)
    {
        if (string.IsNullOrWhiteSpace(options.Value.Service.Name))
            throw new ArgumentException("Service name is required.", nameof(options.Value.Service.Name));

        if (string.IsNullOrWhiteSpace(options.Value.Service.Url))
            throw new ArgumentException("Service url is required.", nameof(options.Value.Service.Url));

        _client = client;
        _serviceDiscoveryRegistration = serviceDiscoveryRegistration;
        _logger = logger;
        _options = options.Value;
        _serviceUrl = new Uri(options.Value.Service.Url);
        _serviceName = options.Value.Service.Name;
        _serviceId = $"{_serviceName}-{Guid.NewGuid():N}";
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Registering a service: '{_serviceId}' in Consul...");
        var result = await _client.Agent.ServiceRegister(GetAgentServiceRegistration(), cancellationToken);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation($"Registered a service: '{_serviceId}' in Consul.");
            return;
        }

        _logger.LogError(
            $"There was an error: {result.StatusCode} when registering a service: '{_serviceId}' in Consul.");
    }

    private AgentServiceRegistration GetAgentServiceRegistration()
    {
        //lazy person's choice
        if (!_options.IsGrpc)
            return new AgentServiceRegistration
            {
                ID = _serviceId,
                Name = _serviceName,
                Address = _serviceUrl.Host,
                Port = _serviceUrl.Port,
                Tags = _serviceDiscoveryRegistration.Tags.ToArray(),
                Check = AgentServiceCheck,
            };
        
        return new AgentServiceRegistration()
        {
            ID = _serviceId,
            Name = _serviceName,
            Address = _serviceUrl.Host,
            Port = _serviceUrl.Port,
            Tags = _serviceDiscoveryRegistration.Tags.ToArray(),
            Check = AgentServiceCheck,
        };
    }
    
    private AgentServiceCheck AgentServiceCheck => _options.IsGrpc
        ? new AgentServiceCheck
        {
            GRPC = $"{_serviceUrl.Host}:{_serviceUrl.Port}",
            GRPCUseTLS = false,
            Name = _serviceId,
            Interval = _options.HealthCheck.Interval,
            DeregisterCriticalServiceAfter = _options.HealthCheck.DeregisterInterval,
            Timeout = TimeSpan.FromSeconds(10)
        }
        : new AgentServiceCheck
        {
            HTTP = $"{_serviceUrl}:{_serviceUrl.Port}",
            Interval = _options.HealthCheck.Interval,
            DeregisterCriticalServiceAfter = _options.HealthCheck.DeregisterInterval,
            Timeout = TimeSpan.FromSeconds(10)
        };

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deregistering a service: '{_serviceId}' in Consul...");
        var result = await _client.Agent.ServiceDeregister(_serviceId, cancellationToken);
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation($"Deregistered a service: '{_serviceId}' in Consul.");
            return;
        }

        _logger.LogError(
            $"There was an error: {result.StatusCode} when deregistering a service: '{_serviceId}' in Consul.");
    }
}