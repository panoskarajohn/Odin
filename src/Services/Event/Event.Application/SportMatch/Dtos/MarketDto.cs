using System.Text.Json.Serialization;

namespace Event.Application.SportMatch.Dtos;

public record MarketDto(string Name, IEnumerable<SelectionDto> Selections);