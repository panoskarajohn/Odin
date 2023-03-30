﻿using Event.Application.SportMatch.Dtos;
using Event.Infrastructure.Mongo;

namespace Event.Application.SportMatch.Mapping;

public static class MatchDocumentToResponse
{
    public static MatchResponseDto ToResponse(this MatchDocument document)
    {
        return new MatchResponseDto(document.Id,
            document.Category,
            document.StartingTime,
            document.MatchName,
            document.Markets?.Select(m => m.ToDto()));
    }

    private static MarketDto ToDto(this MarketDocument document)
    {
        return new MarketDto(document.Name,
            document.Limits.ToDto(),
            document.SelectionDocuments?.Select(s => s.ToDto()));
    }

    private static StakeLimitsDto ToDto(this MarketLimitsDocument document)
    {
        return new StakeLimitsDto(document.MinStake, document.MaxStake);
    }

    private static SelectionDto ToDto(this SelectionDocument document)
    {
        return new SelectionDto(document.Name, document.Price);
    }
}