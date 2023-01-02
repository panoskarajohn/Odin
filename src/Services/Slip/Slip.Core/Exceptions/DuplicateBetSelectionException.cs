using Shared.Types.Exceptions;

namespace Slip.Core.Exceptions;

public class DuplicateBetSelectionException : OdinException
{
    public DuplicateBetSelectionException() : base("You cannot have selections from the same event in the same bet")
    {
    }
}