using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Redis;
using Slip.Core.Repositories;
using Slip.Infrastructure.Repositories;

namespace Slip.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddSlipInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var redisKeys = new RedisKeys();
        return services
            .AddSingleton(redisKeys)
            .AddRedis(configuration)
            .AddScoped<ISlipRepository, RedisSlipRepository>();
    }
}