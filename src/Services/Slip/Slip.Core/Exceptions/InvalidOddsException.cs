using Shared.Types.Exceptions;

namespace Slip.Core.Exceptions;

public class InvalidOddsException : OdinException
{
    public InvalidOddsException(decimal odds) : base($"Invalid odds: {odds}")
    {
    }
}