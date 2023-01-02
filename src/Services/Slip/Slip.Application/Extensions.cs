using Microsoft.Extensions.DependencyInjection;
using Shared.Cqrs;
using Shared.Logging;

namespace Slip.Application;

public static class Extensions
{
    public static IServiceCollection AddSlipApplication(this IServiceCollection services)
    {
        return services
            .AddCqrs()
            .AddLoggingDecorators();
    }
}