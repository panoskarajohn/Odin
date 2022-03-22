using Shared.Domain.Events;

namespace Shared.Domain.Entities;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
    public IEnumerable<IDomainEvent> Events => _events;
    public AggregateId Id { get; protected set; }
    public int Version { get; protected set; }

    public void AddEvent(IDomainEvent @event)
    {
        if (!_events.Any())
        {
            Version++;
        }

        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();
}