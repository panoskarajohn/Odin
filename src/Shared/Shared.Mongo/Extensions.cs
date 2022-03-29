using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Shared.Common;
using Shared.Mongo.Factories;
using Shared.Mongo.Initializers;
using Shared.Mongo.Options;
using Shared.Mongo.Repositories;
using Shared.Mongo.Seeder;

namespace Shared.Mongo;

public static partial class Extensions
{
    private const string SectionName = "mongo";
    
    /// <summary>
    /// Integrates mongo db
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="seederType">Should inherit from IMongoDbSeeder</param>
    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration, Type seederType = null)
    {
        var options = configuration.GetOptions<MongoOptions>(SectionName);
        services.AddSingleton(options);

        services.AddSingleton<IMongoClient>(sp 
            => new MongoClient(options.ConnectionString));

        services.AddTransient(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(options.Database);
        });

        services.AddInitializer<MongoInitializer>();
        services.AddTransient<IMongoSessionFactory, MongoSessionFactory>();
        
        if (seederType is null)
        {
            services.AddTransient<IMongoSeeder, MongoSeeder>();
        }
        else
        {
            services.AddTransient(typeof(IMongoSeeder), seederType);
        }
        
        RegisterConventions();
        
        return services;
    }

    /// <summary>
    /// Registers base implementation to a repository 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="collectionName"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifiable"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services,
        string collectionName)
        where TEntity : IIdentifiable<TIdentifiable>
    {
        services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
        });

        return services;
    }
    private static void RegisterConventions()
    {
        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
        BsonSerializer.RegisterSerializer(typeof(decimal?),
            new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
        ConventionRegistry.Register("mongo", new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(BsonType.String),
        }, _ => true);
    }
}