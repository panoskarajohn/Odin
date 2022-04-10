using RabbitMQ.Client.Events;

namespace Shared.RabbitMq.Plugins;

internal interface IRabbitMqPluginAccessor
{
    void SetSuccessor(Func<object, object, BasicDeliverEventArgs, Task> successor);
}