using Event.Application.SportMatch.Dtos;
using Event.Application.SportMatch.Exceptions;
using Event.Application.SportMatch.Mapping;
using Event.Infrastructure.Mongo;
using Microsoft.Extensions.Logging;
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
        var matchToReturn = await _repository.GetAsync(query.Id);

        if (matchToReturn is null)
            throw new MatchNotFoundException(query.Id);

        return matchToReturn.ToResponse();
    }
}