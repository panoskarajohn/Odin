namespace Shared.Domain;

public interface IEntity<out TId>
{
    TId Id { get; }
    DateTime LastModified { get; }
    bool IsDeleted { get; }
    int? ModifiedBy { get; }
}