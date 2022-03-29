using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class InvalidTeamException : OdinException
{
    public InvalidTeamException(string param) : base($"{param} team should have a valid name")
    {
    }
}