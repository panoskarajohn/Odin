using Event.Match.Mongo.Document;
using Event.Match.Mongo.Mapping.DomainToDocument;
using Event.Match.Repositories;
using Shared.Mongo.Repositories;

namespace Event.Match.Mongo.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly IMongoRepository<MatchDocument, long> _repository;

    public MatchRepository(IMongoRepository<MatchDocument, long> repository)
    {
        _repository = repository;
    }

    public Task<Models.Match> Get(long id)
    {
        throw new NotImplementedException();
    }

    public Task Add(Models.Match match)
    {
        var doc = match.AsDocument();
        return _repository.AddAsync(doc);
    }

    public Task Update(Models.Match match)
    {
        throw new NotImplementedException();
    }
}