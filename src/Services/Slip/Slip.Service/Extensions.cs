using Scrutor;
using Slip.Service.Chain.SlipChain;

namespace Slip.Service;

public static class Extensions
{
    public static IServiceCollection AddSlipChainOfHandlers(this IServiceCollection services)
    {
        // Use Scrutor to scan for all types that implement ISlipChain and register them as a Chain of Responsibility
        services.Scan(scan => scan
            .FromAssemblyOf<ISlipServiceMarker>()
            .AddClasses(classes => classes.AssignableTo<ISlipChain>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}