using Shared.Types.Exceptions;

namespace Slip.Core.Exceptions;

public class InvalidEventIdException : OdinException
{
    public InvalidEventIdException() : base("Invalid event id")
    {
    }
}