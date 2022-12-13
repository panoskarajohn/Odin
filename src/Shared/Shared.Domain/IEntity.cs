namespace Shared.Domain;

public interface IEntity<out TId>
{
    TId Id { get; }
    DateTime LastModified { get; }
    DateTime CreatedAt { get; }
    int? ModifiedBy { get; }
    
}