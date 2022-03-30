namespace Event.Application.Match.Dtos;

public record MarketDto(string Name, IEnumerable<SelectionDto> SelectionDtos);