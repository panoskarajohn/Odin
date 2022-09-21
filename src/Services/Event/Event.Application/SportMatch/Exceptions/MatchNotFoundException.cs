using Shared.Types.Exceptions;

namespace Event.Application.SportMatch.Exceptions;

public class MatchNotFoundException : OdinException
{
    public MatchNotFoundException(long id) : base($"Match with Id: {id} was not found.")
    {
    }
}