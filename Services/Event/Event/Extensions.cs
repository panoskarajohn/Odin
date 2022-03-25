using Event.Match.Mongo.Document;
using Event.Match.Mongo.Repositories;
using Event.Match.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Cqrs;
using Shared.Mongo;

namespace Event;

public static class Extensions
{
    public static IServiceCollection AddEvent(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCqrs();
        services.AddMongo(configuration);
        services.AddMongoRepository<MatchDocument, long>("matches");
        services.AddScoped<IMatchRepository, MatchRepository>();
        
        return services;
    }
}