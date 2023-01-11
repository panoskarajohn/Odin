namespace Slip.Core.Models;

public record UserId
{
    private string Value { get; }

    private UserId(Guid userId)
    {
        if(userId == Guid.Empty)
            throw new UnauthorizedAccessException("User Id cannot be empty");
        Value = userId.ToString();
    }

    public static implicit operator string(UserId category) => category.Value;
    public static implicit operator UserId(Guid value) => new(value);
}    