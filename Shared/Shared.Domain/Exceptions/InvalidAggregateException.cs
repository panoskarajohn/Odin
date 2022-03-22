using Shared.Types.Exceptions;

namespace Shared.Domain.Exceptions;

public class InvalidAggregateIdException : OdinException
{
    public Guid Id { get; }

    public InvalidAggregateIdException(Guid id) : base($"Invalid aggregate id: {id}")
        => Id = id; 
}