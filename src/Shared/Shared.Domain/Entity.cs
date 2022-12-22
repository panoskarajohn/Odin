namespace Shared.Domain;

public abstract class Entity<TId> : IEntity<TId>
{
    protected Entity(TId id) => Id = id;

    protected Entity()
    {
    }

    public long Version { get; set; }
    public TId Id { get; protected set; }
    public DateTime LastModified { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public int? ModifiedBy { get; protected set; }
}