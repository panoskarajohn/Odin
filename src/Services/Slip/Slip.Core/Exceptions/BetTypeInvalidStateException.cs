using Shared.Domain;
using Shared.Types.Exceptions;

namespace Slip.Core.Exceptions;

public class BetTypeInvalidStateException : OdinException
{
    public BetTypeInvalidStateException(IEnumerable<Enumeration> eventStatusEnumerable)
        : base($"Possible values for BetType: {string.Join(",", eventStatusEnumerable.Select(s => s.Name))}")
    {
    }
}