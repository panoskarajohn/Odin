using Scrutor;
using Slip.Service.Chain.SlipChain;

namespace Slip.Service;

public static class Extensions
{
    /// <summary>
    /// For registering slip chain of handlers
    /// Ordering is important
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSlipChainOfHandlers(this IServiceCollection services)
    {
        services.AddScoped<ISlipChain, SlipExists>();
        services.AddScoped<ISlipChain, SlipCheckAgainstGrpc>();

        return services;
    }
}