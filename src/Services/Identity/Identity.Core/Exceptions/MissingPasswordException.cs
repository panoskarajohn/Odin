using Shared.Types.Exceptions;

namespace Identity.Core.Exceptions;

public class MissingPasswordException : OdinException
{
    public MissingPasswordException() : base("Please provide a password")
    {
    }
}