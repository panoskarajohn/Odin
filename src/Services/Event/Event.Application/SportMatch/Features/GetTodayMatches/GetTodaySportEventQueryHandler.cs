﻿using Event.Application.SportMatch.Dtos;
using Event.Application.SportMatch.Mapping;
using Event.Core.Enumerations;
using Event.Infrastructure.Mongo;
using MongoDB.Driver;
using Shared.Cqrs.Queries;
using Shared.Mongo.Repositories;

namespace Event.Application.SportMatch.Features.GetTodayMatches;

public class GetTodaySportEventQueryHandler : IQueryHandler<GetTodayMatchesQuery, IEnumerable<GetTodayMatchesResponse.MatchResponseDto>>
{
    private readonly IMongoRepository<MatchDocument, long> _repository;

    public GetTodaySportEventQueryHandler(IMongoRepository<MatchDocument, long> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetTodayMatchesResponse.MatchResponseDto>> HandleAsync(GetTodayMatchesQuery query,
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var filter = Builders<MatchDocument>.Filter;

        var activeMatches = filter
            .Eq(md => md.Status, Status.Active.Name.ToLower());

        var todayFilter = filter
                              .Gte(sp => sp.StartingTime, today) &
                          filter.Lt(sp => sp.StartingTime, tomorrow);

        var hasMarkets = filter.Where(md => md.Markets.Any());

        var todaySportEvents = await _repository.Collection
            .Find(activeMatches & todayFilter & hasMarkets)
            .SortBy(sp => sp.Category)
            .ThenBy(sp => sp.StartingTime)
            .ToListAsync(cancellationToken);

        if (todaySportEvents is null) return Enumerable.Empty<GetTodayMatchesResponse.MatchResponseDto>();

        return todaySportEvents.Select(t => t.ToTodayMatchesResponse());
    }
}