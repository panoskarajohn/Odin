using MongoDB.Driver;
using Shared.Mongo.Options;
using Shared.Mongo.Seeder;

namespace Shared.Mongo.Initializers;

public class MongoInitializer : IMongoInitializer
{
    private static int _initialized;
    private readonly IMongoDatabase _database;
    private readonly bool _seed;
    private readonly IMongoSeeder _seeder;

    public MongoInitializer(IMongoDatabase database, IMongoSeeder seeder, MongoOptions options)
    {
        _database = database;
        _seeder = seeder;
        _seed = options.Seed;
    }

    public Task InitAsync()
    {
        if (Interlocked.Exchange(ref _initialized, 1) == 1) return Task.CompletedTask;

        return _seed
            ? _seeder.SeedAsync(_database)
            : Task.CompletedTask;
    }
}