using Event.Core.Exceptions;
using Shared.Common;

namespace Event.Core.ValueObjects;

public record StartingTime
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