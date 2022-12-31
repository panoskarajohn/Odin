namespace Event.Core.ValueObjects;

public record MarketLimits
{
    public StakeLimits StakeLimits { get; internal set; } = StakeLimits.Default;
}