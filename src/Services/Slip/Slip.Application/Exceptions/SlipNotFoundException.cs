using Shared.Types.Exceptions;

namespace Slip.Application.Exceptions;

public class SlipNotFoundException : OdinException
{
    public SlipNotFoundException() : base("Slip not found")
    {
    }
}