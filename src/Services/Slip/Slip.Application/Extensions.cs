using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Cqrs;
using Shared.Logging;
using Slip.Infrastructure;

namespace Slip.Application;

public static class Extensions
{
    public static IServiceCollection AddSlipApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddCqrs()
            .AddLoggingDecorators()
            .AddSlipInfrastructure(configuration);
    }
}