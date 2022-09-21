using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class MarketInvalidNameException : OdinException
{
    public MarketInvalidNameException() : base("Please provide a valid name for market")
    {
    }
}