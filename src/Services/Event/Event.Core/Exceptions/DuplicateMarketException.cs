using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class DuplicateMarketException : OdinException
{
    public DuplicateMarketException(string name) : base($"Market with name {name} already exists in this event")
    {
    }
}