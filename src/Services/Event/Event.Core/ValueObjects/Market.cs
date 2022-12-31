using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record Market
{
    public Market(string name,IEnumerable<Selection> selections)
    {
        Name = name;
        Selections = selections ?? throw new MarketShouldContainSelectionsException();
        Limits = new();
    }

    public MarketName Name { get; }
    public MarketLimits Limits { get; private set; } = new();

    public IEnumerable<Selection> Selections { get; }

    public Market WithStakeLimits(StakeLimits limits)
    {
        Limits = Limits with {StakeLimits = limits};
        return this;
    }
}