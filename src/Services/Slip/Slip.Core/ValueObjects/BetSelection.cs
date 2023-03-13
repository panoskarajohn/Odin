using Slip.Core.Exceptions;

namespace Slip.Core.ValueObjects;

public record BetSelection
{
    private BetSelection(long eventId, string marketName, string outcome, decimal odds)
    {
        if (eventId == 0)
            throw new InvalidEventIdException();

        if (string.IsNullOrEmpty(marketName))
            throw new InvalidMarketNameException();

        if (string.IsNullOrEmpty(outcome))
            throw new InvalidOutcomeNameException();

        if (odds <= 0)
            throw new InvalidOddsException(odds);

        EventId = eventId;
        MarketName = marketName;
        Outcome = outcome;
        Odds = odds;
    }

    public long EventId { get; }
    public string MarketName { get; }
    public string Outcome { get; }

    public decimal Odds { get; }

    public static BetSelection Create(long eventId, string marketName, string outcome, decimal odds)
    {
        return new BetSelection(eventId, marketName, outcome, odds);
    }
}