using MongoDB.Driver;

namespace Shared.Mongo.Seeder;

public interface IMongoSeeder
{
    Task SeedAsync(IMongoDatabase database);
}