using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class InvalidMarketNameException : OdinException
{
    public InvalidMarketNameException() : base("Market name should not be an empty value")
    {
    }
}