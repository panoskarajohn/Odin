using Event.Core.Repositories;
using Event.Infrastructure.Mongo;
using Event.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Mongo;

namespace Event.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddEventInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongo(configuration);
        services.AddMongoRepository<MatchDocument, long>("matches");
        services.AddMongoRepository<MarketTemplateDocument, long>("marketTemplates");
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IMarketTemplateRepository, MarketTemplateRepository>();
        return services;
    }
}