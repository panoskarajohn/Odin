using Shared.Cqrs.Commands;
using Shared.Cqrs.Events;

namespace Shared.MessageBroker.Cqrs.Dispatchers;

internal sealed class ServiceBusMessageDispatcher : ICommandDispatcher, IEventDispatcher
{
    private readonly ICorrelationContextAccessor _accessor;
    private readonly IBusPublisher _busPublisher;

    public ServiceBusMessageDispatcher(IBusPublisher busPublisher, ICorrelationContextAccessor accessor)
    {
        _busPublisher = busPublisher;
        _accessor = accessor;
    }

    public Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand
    {
        return _busPublisher.SendAsync(command, _accessor.CorrelationContext);
    }

    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IEvent
    {
        return _busPublisher.PublishAsync(@event, _accessor.CorrelationContext);
    }
}