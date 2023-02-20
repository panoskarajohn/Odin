using Event.Core.Models;
using Event.Core.Repositories;
using Event.Infrastructure.Mapping;
using Event.Infrastructure.Mapping.ToEntity;
using Event.Infrastructure.Mongo;
using MongoDB.Driver;
using Shared.Mongo.Repositories;

namespace Event.Infrastructure.Repositories;

public class MarketTemplateRepository : IMarketTemplateRepository
{
    private readonly IMongoRepository<MarketTemplateDocument, long> _repository;

    public MarketTemplateRepository(IMongoRepository<MarketTemplateDocument, long> repository)
    {
        _repository = repository;
    }

    public async Task<MarketTemplate> Get(long id)
    {
        var document = await _repository.GetAsync(id).ConfigureAwait(false);
        return document.ToEntity();
    }

    public async Task<MarketTemplate?> Get(string marketName, string category)
    {
        var filter = Builders<MarketTemplateDocument>.Filter.Eq(x => x.MarketName, marketName) &
                     Builders<MarketTemplateDocument>.Filter.Eq(x => x.Category, category);
        var document = await _repository.Collection.FindAsync(filter).ConfigureAwait(false);
        return document
            .SingleOrDefault()?
            .ToEntity();
    }

    public Task Add(MarketTemplate marketTemplate)
    {
        var docuement = marketTemplate.ToDocument();
        return _repository.AddAsync(docuement);
    }

    public Task Update(MarketTemplate marketTemplate)
    {
        return _repository
            .UpdateAsync(marketTemplate.ToDocument(),
            m => m.Id == marketTemplate.Id && m.Version < marketTemplate.Version);
    }
}