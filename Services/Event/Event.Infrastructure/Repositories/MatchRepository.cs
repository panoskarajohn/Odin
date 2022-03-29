using Event.Core.Models;
using Event.Core.Repositories;
using Event.Infrastructure.Mapping;
using Event.Infrastructure.Mongo;
using Shared.Mongo.Repositories;

namespace Event.Infrastructure.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly IMongoRepository<MatchDocument, long> _repository;

    public MatchRepository(IMongoRepository<MatchDocument, long> repository)
    {
        _repository = repository;
    }

    public Task<Match> Get(long id)
    {
        throw new NotImplementedException();
    }

    public Task Add(Core.Models.Match match)
    {
        var doc = match.AsDocument();
        return _repository.AddAsync(doc);
    }

    public Task Update(Match match)
    {
        throw new NotImplementedException();
    }
}