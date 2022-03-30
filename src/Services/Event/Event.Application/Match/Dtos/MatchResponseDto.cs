namespace Event.Application.Match.Dtos;

public record MatchResponseDto(long Id, string Category, DateTime StartingTime, string MatchName)
{
}