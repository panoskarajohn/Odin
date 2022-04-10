using RabbitMQ.Client.Events;

namespace Shared.RabbitMq.Plugins;

public abstract class RabbitMqPlugin : IRabbitMqPlugin, IRabbitMqPluginAccessor
{
    private Func<object, object, BasicDeliverEventArgs, Task> _successor;

    public abstract Task HandleAsync(object message, object correlationContext,
        BasicDeliverEventArgs args);

    void IRabbitMqPluginAccessor.SetSuccessor(Func<object, object, BasicDeliverEventArgs, Task> successor)
        => _successor = successor;

    public Task Next(object message, object correlationContext, BasicDeliverEventArgs args)
        => _successor(message, correlationContext, args);
}