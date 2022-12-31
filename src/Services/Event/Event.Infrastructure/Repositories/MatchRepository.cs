using Event.Core.Models;
using Event.Core.Repositories;
using Event.Infrastructure.Mapping;
using Event.Infrastructure.Mapping.ToEntity;
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

    public async Task<Match?> Get(long id)
    {
        var doc = await _repository.GetAsync(id);
        return doc?.AsEntity();
    }

    public Task Add(Core.Models.Match match)
    {
        var doc = match.AsDocument();
        return _repository.AddAsync(doc);
    }

    public Task Update(Match match)
    {
        return _repository
            .UpdateAsync(match.AsDocument(), m
                => m.Id == match.Id && m.Version < match.Version);
    }
}