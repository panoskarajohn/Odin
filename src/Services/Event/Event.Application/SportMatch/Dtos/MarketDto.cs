namespace Event.Application.SportMatch.Dtos;

public record MarketDto(string Name,
    StakeLimitsDto StakeLimits,
    IEnumerable<SelectionDto> Selections);