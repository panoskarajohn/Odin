﻿using Event.Application.SportMatch.Exceptions;
using Event.Core.DomainEvents;
using Event.Core.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;
using Shared.Web.Context;

namespace Event.Application.SportMatch.Features.RemoveMarkets;

public class RemoveMarketCommandHandler : ICommandHandler<RemoveMarketsCommand>
{
    private readonly ILogger<RemoveMarketCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;
    private readonly IContext _context;
    public RemoveMarketCommandHandler(ILogger<RemoveMarketCommandHandler> logger, IMatchRepository matchRepository, IContext context)
    {
        _logger = logger;
        _matchRepository = matchRepository;
        _context = context;
    }

    public async Task HandleAsync(RemoveMarketsCommand command, CancellationToken cancellationToken = default)
    {
        var match = await _matchRepository.Get(command.MatchId);

        if (match is null)
            throw new MatchNotFoundException(command.MatchId);

        match.RemoveMarkets(command.MarketNames);
        match.AddDomainEvent(new MarketsRemovedEvent());

        await _matchRepository.Update(match, _context.Identity.Id.ToString());
    }
}