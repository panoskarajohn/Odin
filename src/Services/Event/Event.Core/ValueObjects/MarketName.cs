using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record MarketName
{
    private MarketName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidMarketNameException();

        Value = value.ToLower();
    }

    private string Value { get; }

    public static implicit operator string(MarketName category)
    {
        return category.Value;
    }

    public static implicit operator MarketName(string value)
    {
        return new(value);
    }
}