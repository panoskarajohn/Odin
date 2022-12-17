using Shared.Types.Exceptions;

namespace Identity.Core.Exceptions;

public class InvalidPasswordException : OdinException
{
    public InvalidPasswordException() : base("Invalid password")
    {
    }
}