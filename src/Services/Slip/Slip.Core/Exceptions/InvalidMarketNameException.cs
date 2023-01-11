using Shared.Types.Exceptions;

namespace Slip.Core.Exceptions;

public class InvalidMarketNameException : OdinException
{
    public InvalidMarketNameException() : base("Invalid market name")
    {
    }
}