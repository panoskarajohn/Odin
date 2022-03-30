using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class MarketNotFoundException : OdinException
{
    public MarketNotFoundException(string marketName) : base($"Market with name {marketName} was not found")
    {
    }
}