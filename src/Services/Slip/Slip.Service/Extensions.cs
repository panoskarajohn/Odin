using Scrutor;
using Slip.Service.Chain.SlipChain;

namespace Slip.Service;

public static class Extensions
{
    public static IServiceCollection AddSlipChainOfHandlers(this IServiceCollection services)
    {
        services.AddScoped<ISlipChain, SlipExists>();
        services.AddScoped<ISlipChain, SlipCheckAgainstGrpc>();

        return services;
    }
}