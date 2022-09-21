using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class NullMarketException : OdinException
{
    public NullMarketException() : base("Market should not be empty")
    {
    }
}