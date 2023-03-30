using MongoDB.Driver;

namespace Shared.Mongo.Factories;

internal sealed class MongoSessionFactory : IMongoSessionFactory
{
    private readonly IMongoClient _client;

    public MongoSessionFactory(IMongoClient client)
    {
        _client = client;
    }

    public Task<IClientSessionHandle> CreateAsync()
    {
        return _client.StartSessionAsync();
    }
}