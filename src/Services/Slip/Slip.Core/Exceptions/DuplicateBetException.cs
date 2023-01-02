using Shared.Types.Exceptions;

namespace Slip.Core.Exceptions;

public class DuplicateBetException : OdinException
{
    public DuplicateBetException() : base("Duplicate bet")
    {
    }
}