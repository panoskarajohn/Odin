using System.Linq.Expressions;
using MongoDB.Driver;

namespace Shared.Mongo.Repositories;

internal class MongoRepository<TEntity, TIdentifiable> : IMongoRepository<TEntity, TIdentifiable>
    where TEntity : IIdentifiable<TIdentifiable>
{
    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        Collection = database.GetCollection<TEntity>(collectionName);
    }

    public IMongoCollection<TEntity> Collection { get; }

    public Task<TEntity> GetAsync(TIdentifiable id)
    {
        return Collection.Find(e
                => e.Id.Equals(id))
            .SingleOrDefaultAsync();
    }

    public Task AddAsync(TEntity entity) => Collection.InsertOneAsync(entity);

    public Task UpdateAsync(TEntity entity)
        => Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);

    public Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        => Collection.ReplaceOneAsync(predicate, entity);

    public Task DeleteAsync(TIdentifiable id)
        => Collection.DeleteOneAsync(e => e.Id.Equals(id));
}