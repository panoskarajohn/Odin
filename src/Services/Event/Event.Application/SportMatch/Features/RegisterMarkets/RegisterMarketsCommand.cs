using Event.Application.SportMatch.Dtos;
using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.RegisterMarkets;

public record RegisterMarketsCommand(long MatchId, IEnumerable<MarketDto> Markets) : ICommand;