using Shared.Domain;
using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class StatusInvalidStateException : OdinException
{
    public StatusInvalidStateException(IEnumerable<Enumeration> eventStatusEnumerable) 
        : base($"Possible values for EventStatus: {String.Join(",", eventStatusEnumerable.Select(s => s.Name))}")
    {
    }
}