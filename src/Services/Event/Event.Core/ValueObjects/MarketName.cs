using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record MarketName
{
    private string Value { get; }

    private MarketName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidMarketNameException();
        
        Value = value.ToLower();
    }
    
    public static implicit operator string(MarketName category) => category.Value;
    public static implicit operator MarketName(string value) => new(value);
}