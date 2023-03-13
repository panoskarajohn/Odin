using Microsoft.Extensions.DependencyInjection;

namespace Shared.Grpc;

public static class Extensions
{
    public static void AddCustomGrpc(this IServiceCollection services)
    {
        services.AddGrpc(options => { options.Interceptors.Add<ExceptionInterceptor>(); });

        services.AddSingleton<ExceptionInterceptor>();
    }
}