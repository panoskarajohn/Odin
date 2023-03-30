using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Shared.Grpc;

public static class Extensions
{
    public static void AddCustomGrpc(this IServiceCollection services)
    {
        services
            .AddGrpcHealthChecks()
            .AddCheck("Sample", () => HealthCheckResult.Healthy());
        
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionInterceptor>();
        });
        services.AddSingleton<ExceptionInterceptor>();
    }
}