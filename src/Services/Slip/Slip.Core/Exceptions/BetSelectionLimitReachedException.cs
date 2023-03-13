namespace Slip.Core.Exceptions;

public class BetSelectionLimitReachedException : Exception
{
    public BetSelectionLimitReachedException(int numberOfSelections) : base(
        $"Bet selection limit reached. Maximum number of selections is {numberOfSelections}.")
    {
    }
}