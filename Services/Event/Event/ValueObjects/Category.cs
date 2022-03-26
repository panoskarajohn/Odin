using Shared.Types.Exceptions;

namespace Event.ValueObjects;

public record Category
{
    private string Value { get; }

    private Category(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidCategoryException();
        
        Value = value;
    }

    public static implicit operator string(Category category) => category.Value;
    public static implicit operator Category(string value) => new(value);
}

public class InvalidCategoryException : OdinException
{
    public InvalidCategoryException() : base("Category should not be an empty value")
    {
    }
} 