using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class MarketShouldContainSelectionsException : OdinException
{
    public MarketShouldContainSelectionsException() : base("Market should contain at least one selection")
    {
    }
}