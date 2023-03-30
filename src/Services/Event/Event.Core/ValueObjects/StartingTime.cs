using Event.Core.Exceptions;
using Shared.Common;

namespace Event.Core.ValueObjects;

public record StartingTime
{
    public StartingTime(DateTime value)
    {
        if (value.UtcTimeIsFromPast()) throw new InvalidStartingTimeException(value);
        Value = value;
    }

    public DateTime Value { get; }

    public static implicit operator DateTime(StartingTime category)
    {
        return category.Value;
    }

    public static implicit operator StartingTime(DateTime value)
    {
        return new(value);
    }
}