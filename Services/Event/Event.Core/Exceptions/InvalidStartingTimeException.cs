using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class InvalidStartingTimeException : OdinException
{
    public InvalidStartingTimeException(DateTime dateTime) : base($"{dateTime:G} is not a valid date time!")
    {
    }
}