namespace Event.Application.Match.Dtos;

public record MatchesResponseDto(long Id, string Category, DateTime StartingTime, string MatchName)
{
}