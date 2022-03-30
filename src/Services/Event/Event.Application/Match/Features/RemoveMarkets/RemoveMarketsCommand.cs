using Shared.Cqrs.Commands;

namespace Event.Application.Match.Features.RemoveMarkets;

public record RemoveMarketsCommand(long MatchId, IEnumerable<string> MarketNames) : ICommand;