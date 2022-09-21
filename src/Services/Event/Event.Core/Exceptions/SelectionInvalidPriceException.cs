using Event.Core.ValueObjects;
using Shared.Types.Exceptions;

namespace Event.Core.Exceptions;

public class SelectionInvalidPriceException : OdinException
{
    public SelectionInvalidPriceException() : base($"Selection price cannot be smaller than {Selection.MinPrice} " +
                                                   $"and larger than {Selection.MaxPrice}")
    {
    }
}