using Event.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Cqrs;

namespace Event.Application;

public static class Extensions
{
    public static IServiceCollection AddEventApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCqrs();
        services.AddEventInfrastructure(configuration);
        
        return services;
    }
}