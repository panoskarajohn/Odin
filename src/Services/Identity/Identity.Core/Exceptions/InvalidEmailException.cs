using Shared.Types.Exceptions;

namespace Identity.Core.Exceptions;

public class InvalidEmailException : OdinException
{
    public InvalidEmailException() : base("Invalid email address")
    {
    }
}