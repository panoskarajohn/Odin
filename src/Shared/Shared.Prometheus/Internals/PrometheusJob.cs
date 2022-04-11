using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus.DotNetRuntime;

namespace Shared.Prometheus.Internals;

internal sealed class PrometheusJob : IHostedService
{
    private readonly bool _enabled;
    private readonly ILogger<PrometheusJob> _logger;
    private IDisposable _collector;

    public PrometheusJob(PrometheusOptions options, ILogger<PrometheusJob> logger)
    {
        _enabled = options.Enabled;
        _logger = logger;
        _logger.LogInformation($"Prometheus integration is {(_enabled ? "enabled" : "disabled")}.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_enabled)
        {
            _collector = DotNetRuntimeStatsBuilder
                .Customize()
                .WithContentionStats()
                .WithJitStats()
                .WithThreadPoolStats()
                .WithThreadPoolStats()
                .WithGcStats()
                .WithExceptionStats()
                .StartCollecting();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _collector?.Dispose();

        return Task.CompletedTask;
    }
}