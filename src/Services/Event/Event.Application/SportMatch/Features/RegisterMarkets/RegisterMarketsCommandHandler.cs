using Event.Application.SportMatch.Exceptions;
using Event.Core.DomainEvents;
using Event.Core.Repositories;
using Event.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;
using Shared.Web.Context;

namespace Event.Application.SportMatch.Features.RegisterMarkets;

public class RegisterMarketsCommandHandler : ICommandHandler<RegisterMarketsCommand>
{
    private readonly ILogger<RegisterMarketsCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;
    private readonly IMarketTemplateRepository _marketTemplateRepository;
    private readonly IContext _context;

    public RegisterMarketsCommandHandler(ILogger<RegisterMarketsCommandHandler> logger,
        IMatchRepository matchRepository, IMarketTemplateRepository marketTemplateRepository, IContext context)
    {
        _logger = logger;
        _matchRepository = matchRepository;
        _marketTemplateRepository = marketTemplateRepository;
        _context = context;
    }

    public async Task HandleAsync(RegisterMarketsCommand command, CancellationToken cancellationToken = default)
    {
        var match = await _matchRepository.Get(command.MatchId);

        if (match is null)
            throw new MatchNotFoundException(command.MatchId);

        var marketDtos = command.Markets;

        var markets = new List<Market>();

        foreach (var marketDto in marketDtos)
        {
            var marketSelections = marketDto
                .Selections?
                .Select(s => new Selection(s.Name, s.Price));
            var market = new Market(marketDto.Name, marketSelections!);

            var marketTemplate = await _marketTemplateRepository.Get(market.Name, match.Category);

            if (marketTemplate is null)
                throw new TemplateNotFound(market.Name, match.Category);
            
            UpdateLimitsFromTemplate(ref market, marketTemplate);
            markets.Add(market);
        }

        match.AppendMarkets(markets);
        match.AddDomainEvent(new MarketsRegisteredEvent());
        await _matchRepository.Update(match, _context.Identity.Id.ToString());
    }
    
    private void UpdateLimitsFromTemplate(ref Market market, Core.Models.MarketTemplate? marketTemplate)
    {
        if (marketTemplate is null) return;

        market.WithStakeLimits(marketTemplate.StakeLimits);
    }
}