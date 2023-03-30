namespace Event.Application.SportMatch.Features.GetTodayMatches;

public class GetTodayMatchesResponse
{
    public record MatchResponseDto(long Id,
        string Category,
        DateTime StartingTime,
        string MatchName,
        IEnumerable<MarketResponseDto> Markets);

    public record MarketResponseDto(string Name,
        IEnumerable<SelectionResponseDto> Selections);

    public record SelectionResponseDto(string Name, decimal Price);
}