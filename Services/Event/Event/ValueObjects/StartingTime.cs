using JetBrains.Annotations;
using Shared.Common;
using Shared.Types.Exceptions;

namespace Event.ValueObjects;

public class StartingTime
{
    public DateTime Value { get; }

    private StartingTime(DateTime value)
    {
        if (value.UtcTimeIsFromFuture()) throw new InvalidStartingTimeException(value);
        Value = value;
    }

    public static implicit operator DateTime(StartingTime category) => category.Value;
    public static implicit operator StartingTime(DateTime value) => new(value);
}

public class InvalidStartingTimeException : OdinException
{
    public InvalidStartingTimeException(DateTime dateTime) : base($"{dateTime:G} is not a valid date time!")
    {
    }
} 