using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Event;

public static class Extensions
{
    public static IServiceCollection AddEvent(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}