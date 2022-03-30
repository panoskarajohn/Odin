using Event.Core.Exceptions;
using Shared.Common;

namespace Event.Core.ValueObjects;

public record StartingTime
{
    public StartingTime(DateTime value)
    {
        if (value.UtcTimeIsFromFuture()) throw new InvalidStartingTimeException(value);
        Value = value;
    }

    public DateTime Value { get; }
}