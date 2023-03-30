using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record StakeLimits
{
    private const decimal DefaultMinStake = 0.30m;
    private const decimal DefaultMaxStake = 2000.0m;

    private StakeLimits(decimal minStake = DefaultMinStake, decimal maxStake = DefaultMaxStake)
    {
        if (minStake < DefaultMinStake || minStake > DefaultMaxStake)
            throw new MinMaxStakeOutOfRange(DefaultMinStake, DefaultMaxStake);
        if (maxStake < DefaultMinStake || maxStake > DefaultMaxStake)
            throw new MinMaxStakeOutOfRange(DefaultMinStake, DefaultMaxStake);
        if (minStake > maxStake)
            throw new MinMaxStakeOutOfRange(DefaultMinStake, DefaultMaxStake);

        MinStake = minStake;
        MaxStake = maxStake;
    }

    public decimal MinStake { get; }
    public decimal MaxStake { get; }

    public static StakeLimits Default => new();

    public static StakeLimits Create(decimal minStake, decimal maxStake)
    {
        return new StakeLimits(minStake, maxStake);
    }
}