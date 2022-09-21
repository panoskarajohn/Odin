using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class SelectionInvalidNameException : OdinException
{
    public SelectionInvalidNameException() : base("Outcome name cannot be empty")
    {
    }
}