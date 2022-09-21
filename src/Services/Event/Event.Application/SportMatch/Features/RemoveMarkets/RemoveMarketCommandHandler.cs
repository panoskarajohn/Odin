using Event.Application.SportMatch.Exceptions;
using Event.Core.DomainEvents;
using Event.Core.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.RemoveMarkets;

public class RemoveMarketCommandHandler : ICommandHandler<RemoveMarketsCommand>
{
    private readonly ILogger<RemoveMarketCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;

    public RemoveMarketCommandHandler(ILogger<RemoveMarketCommandHandler> logger, IMatchRepository matchRepository)
    {
        _logger = logger;
        _matchRepository = matchRepository;
    }

    public async Task HandleAsync(RemoveMarketsCommand command, CancellationToken cancellationToken = default)
    {
        var match = await _matchRepository.Get(command.MatchId);

        if (match is null)
            throw new MatchNotFoundException(command.MatchId);

        match.RemoveMarkets(command.MarketNames);
        match.AddDomainEvent(new MarketsRemovedEvent());

        await _matchRepository.Update(match);
    }
}