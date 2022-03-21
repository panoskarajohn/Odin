using MongoDB.Driver;

namespace Shared.Mongo.Factories;

public interface IMongoSessionFactory
{
    Task<IClientSessionHandle> CreateAsync();
}