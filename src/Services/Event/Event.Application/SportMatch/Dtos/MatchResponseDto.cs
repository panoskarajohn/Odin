using System.Text.Json.Serialization;

namespace Event.Application.SportMatch.Dtos;

public record MatchResponseDto(long Id, 
    string Category, 
    DateTime StartingTime, 
    string MatchName,
    IEnumerable<MarketDto> Markets)
{
}