namespace Shared.Domain;

public interface IEntity<out TId>
{
    TId Id { get; }
    DateTime LastModified { get; }
    int? ModifiedBy { get; }
}