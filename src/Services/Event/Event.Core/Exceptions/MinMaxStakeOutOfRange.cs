using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class MinMaxStakeOutOfRange : OdinException
{
    public MinMaxStakeOutOfRange(decimal minStake, decimal maxStake) : base(
        $"Min stake is {minStake} and max stake is {maxStake}")
    {
    }
}