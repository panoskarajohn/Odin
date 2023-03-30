namespace Slip.Core.Models;

public record UserId
{
    private UserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new UnauthorizedAccessException("User Id cannot be empty");
        Value = userId.ToString();
    }

    private string Value { get; }

    public static implicit operator string(UserId category)
    {
        return category.Value;
    }

    public static implicit operator UserId(Guid value)
    {
        return new(value);
    }
}