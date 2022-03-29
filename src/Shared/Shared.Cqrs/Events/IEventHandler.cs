namespace Shared.Cqrs.Events;

public interface IEventHandler<in TEvent> where TEvent : class, IEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}

public interface IRejectedEvent : IEvent
{
    string Reason { get; }
    string Code { get; }
}