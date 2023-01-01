using Event.Infrastructure.Mongo;

namespace Event.Application.SportMatch.Features.GetTodayMatches;

public static class GetTodayMatchesResponseMappings
{
    public static GetTodayMatchesResponse.MatchResponseDto ToTodayMatchesResponse(this MatchDocument document)
    {
        return new GetTodayMatchesResponse.MatchResponseDto(document.Id,
            document.Category,
            document.StartingTime,
            document.MatchName,
            document.Markets?.Select(m => m.ToDto()));
    }

    static GetTodayMatchesResponse.MarketResponseDto ToDto(this MarketDocument document)
    {
        return new GetTodayMatchesResponse.MarketResponseDto(document.Name,
            document.SelectionDocuments?.Select(s => s.ToDto()));
    }

    static GetTodayMatchesResponse.SelectionResponseDto ToDto(this SelectionDocument document)
    {
        return new GetTodayMatchesResponse.SelectionResponseDto(document.Name, document.Price);
    }
}