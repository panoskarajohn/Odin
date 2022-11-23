using Event.Application.SportMatch.Exceptions;
using Event.Core.Repositories;
using Event.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.RegisterMarkets;

public class RegisterMarketsCommandHandler : ICommandHandler<RegisterMarketsCommand>
{
    private readonly ILogger<RegisterMarketsCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;

    public RegisterMarketsCommandHandler(ILogger<RegisterMarketsCommandHandler> logger,
        IMatchRepository matchRepository)
    {
        _logger = logger;
        _matchRepository = matchRepository;
    }

    public async Task HandleAsync(RegisterMarketsCommand command, CancellationToken cancellationToken = default)
    {
        var match = await _matchRepository.Get(command.MatchId);

        if (match is null)
            throw new MatchNotFoundException(command.MatchId);

        var marketDtos = command.MarketDtos;

        var markets = new List<Market>();

        foreach (var marketDto in marketDtos)
        {
            var marketSelections = marketDto
                .Selections?
                .Select(s => new Selection(s.Name, s.Price));
            var market = new Market(marketDto.Name, marketSelections!);
            markets.Add(market);
        }

        match.AppendMarkets(markets);
        await _matchRepository.Update(match);
    }
}