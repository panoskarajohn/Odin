using Event.Core.Models;
using Event.Core.ValueObjects;
using Event.Infrastructure.Mongo;

namespace Event.Infrastructure.Mapping;

public static class MatchToDocument
{
    public static MatchDocument AsDocument(this Match match, string userId = null)
    {
        (string home, string away) = match.MatchName;

        var matchDocument = new MatchDocument(match.Id,
            match.Category,
            match.StartingTime,
            match.MatchName.Value,
            home, away,
            match.Status.Name, 
            match.Version,
            userId);

        if (match.Markets.Any())
        {
            matchDocument.Markets = match.Markets.Select(m => m.AsDocument());
        }

        return matchDocument;
    }

    public static MarketDocument AsDocument(this Market market)
    {
        return new MarketDocument(market.Name,
            market.Selections.Select(s => s.AsDocument()), 
            market.Limits.AsDocument());
    }

    public static MarketLimitsDocument AsDocument(this MarketLimits marketLimits)
    {
        return new MarketLimitsDocument(marketLimits.StakeLimits.MinStake, marketLimits.StakeLimits.MaxStake);
    }

    public static SelectionDocument AsDocument(this Selection selection)
    {
        return new SelectionDocument(selection.Name, selection.Price);
    }
}