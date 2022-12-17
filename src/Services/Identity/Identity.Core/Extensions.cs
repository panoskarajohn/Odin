using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Core;

public static class Extensions
{
    public static IServiceCollection AddIdentityCore(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}