using RabbitMQ.Client.Events;

namespace Shared.RabbitMq.Plugins;

public interface IRabbitMqPlugin
{
    Task HandleAsync(object message, object correlationContext, BasicDeliverEventArgs args);
}