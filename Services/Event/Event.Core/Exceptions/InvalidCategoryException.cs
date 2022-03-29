using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class InvalidCategoryException : OdinException
{
    public InvalidCategoryException() : base("Category should not be an empty value")
    {
    }
}