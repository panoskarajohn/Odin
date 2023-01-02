using Shared.Types.Exceptions;

namespace Slip.Core.Exceptions;

public class InvalidOutcomeNameException : OdinException
{
    public InvalidOutcomeNameException() : base("Invalid outcome name")
    {
    }
}