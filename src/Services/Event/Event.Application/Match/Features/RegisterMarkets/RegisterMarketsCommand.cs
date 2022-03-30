using Event.Application.Match.Dtos;
using Shared.Cqrs.Commands;

namespace Event.Application.Match.Features.RegisterMarkets;

public record RegisterMarketsCommand(long MatchId, IEnumerable<MarketDto> MarketDtos) : ICommand;