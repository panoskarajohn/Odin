using Shared.Types.Exceptions;

namespace Slip.Application.Exceptions;

public class NoBetsException : OdinException
{
    public NoBetsException() : base("No bets found")
    {
    }
}