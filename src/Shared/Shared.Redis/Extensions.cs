using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Shared.Redis;

public static class Extensions
{
    private const string RedisSection = "redis";
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection(RedisSection);
        var options = new RedisOptions();
        section.Bind(options);
        services.Configure<RedisOptions>(section);
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options.ConnectionString));
        
        return services;
    }
}