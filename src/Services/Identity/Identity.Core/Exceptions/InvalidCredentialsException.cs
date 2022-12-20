using Shared.Types.Exceptions;

namespace Identity.Core.Exceptions;

public class InvalidCredentialsException : OdinException
{
    public InvalidCredentialsException() : base("The email password combination are incorrect")
    {
    }
}