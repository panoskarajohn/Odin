using Shared.Types.Exceptions;

namespace Identity.Core.Exceptions;

public class EmailInUseException : OdinException
{
    public EmailInUseException() : base("Email is already in use")
    {
    }
}