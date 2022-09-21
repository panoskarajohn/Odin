using MongoDB.Driver;

namespace Shared.Mongo.Seeder;

internal class MongoSeeder : IMongoSeeder
{
    public async Task SeedAsync(IMongoDatabase database)
    {
        await CustomSeedAsync(database);
    }

    protected virtual async Task CustomSeedAsync(IMongoDatabase database)
    {
        var cursor = await database.ListCollectionsAsync();
        var collections = await cursor.ToListAsync();
        if (collections.Any())
        {
            return;
        }

        await Task.CompletedTask;
    }
}