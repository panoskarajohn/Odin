namespace Slip.Core.Models;

public record Stake
{
    private decimal Value { get; }

    internal const decimal MaxStakeValue = 100_000m;

    private Stake(decimal value)
    {
        if(value <= 0 || value > MaxStakeValue)
            throw new ArgumentOutOfRangeException(nameof(value), $"Stake value must be between 0 and {MaxStakeValue}");

        Value = value;
    }

    public static implicit operator decimal(Stake category) => category.Value;
    public static implicit operator Stake(decimal value) => new(value);
}