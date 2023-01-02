using Shared.Types.Exceptions;
using Slip.Core.Models;

namespace Slip.Core.Exceptions;

public class InvalidStakeException : OdinException
{
    public InvalidStakeException() : base($"Stake must be positive and less than {Stake.MaxStakeValue}")
    {
        
    }
}