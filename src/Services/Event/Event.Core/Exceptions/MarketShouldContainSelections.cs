using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class MarketShouldContainSelections : OdinException
{
    public MarketShouldContainSelections() : base("Market should contain at least one selection")
    {
    }
}