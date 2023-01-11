namespace Event.Infrastructure.Mongo;

public class MarketLimitsDocument
{
    public MarketLimitsDocument(decimal minStake, decimal maxStake)
    {
        MinStake = minStake;
        MaxStake = maxStake;
    }

    public decimal MinStake { get; }
    public decimal MaxStake { get; }
}