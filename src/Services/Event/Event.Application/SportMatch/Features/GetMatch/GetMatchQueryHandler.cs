using Event.Application.SportMatch.Dtos;
using Event.Application.SportMatch.Exceptions;
using Event.Application.SportMatch.Mapping;
using Event.Infrastructure.Mongo;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shared.Cqrs.Queries;
using Shared.Mongo.Repositories;

namespace Event.Application.SportMatch.Features.GetMatch;

public class GetMatchQueryHandler : IQueryHandler<GetMatchQuery, MatchResponseDto>
{
    private readonly ILogger<GetMatchQueryHandler> _logger;
    private readonly IMongoRepository<MatchDocument, long> _repository;

    public GetMatchQueryHandler(IMongoRepository<MatchDocument, long> repository, ILogger<GetMatchQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<MatchResponseDto> HandleAsync(GetMatchQuery query, CancellationToken cancellationToken = default)
    {
        var match = await _repository.Collection
            .FindAsync((s) => s.Id.Equals(query.Id), cancellationToken: cancellationToken);

        var matchToReturn = await match.FirstAsync();

        if (matchToReturn is null)
            throw new MatchNotFoundException(query.Id);

        return matchToReturn.ToResponse();
    }
}