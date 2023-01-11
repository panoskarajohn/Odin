using Event.Application.SportMatch.Dtos;
using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.RegisterMarkets;

public record RegisterMarketsCommand(long MatchId, IEnumerable<MarketsToRegister> Markets) : ICommand;

public record MarketsToRegister(string Name,
    IEnumerable<SelectionDto> Selections);