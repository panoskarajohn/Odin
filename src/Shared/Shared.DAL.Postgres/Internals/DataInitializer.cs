using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.DAL.Postgres.Internals;

public sealed class DataInitializer : IHostedService
{
    private readonly ILogger<DataInitializer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DataInitializer(ILogger<DataInitializer> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var initializers = scope.ServiceProvider.GetServices<IDataInitializer>();
        foreach (var initializer in initializers)
            try
            {
                _logger.LogInformation($"Running the initializer: {initializer.GetType().Name}...");
                await initializer.InitAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
            finally
            {
                _logger.LogInformation($"Exiting the initializer: {initializer.GetType().Name}...");
            }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}