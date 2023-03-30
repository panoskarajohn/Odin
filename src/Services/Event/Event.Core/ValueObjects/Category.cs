using Event.Core.Exceptions;

namespace Event.Core.ValueObjects;

public record Category
{
    private Category(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidCategoryException();

        Value = value.ToLower();
    }

    private string Value { get; }

    public static implicit operator string(Category category)
    {
        return category.Value;
    }

    public static implicit operator Category(string value)
    {
        return new(value);
    }
}