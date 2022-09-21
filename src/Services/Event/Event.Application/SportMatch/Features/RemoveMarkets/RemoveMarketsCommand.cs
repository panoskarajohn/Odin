using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.RemoveMarkets;

public record RemoveMarketsCommand(long MatchId, IEnumerable<string> MarketNames) : ICommand;