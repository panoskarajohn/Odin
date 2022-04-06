namespace Shared.RabbitMq.Subscribers;

public class MessageContract : IMessageContract
{
    public MessageContract(Type type, string exchange, string routingKey,
        Func<IServiceProvider, object, object, Task> handle)
    {
        Type = type;
        Exchange = exchange;
        RoutingKey = routingKey;
        Handle = handle;
    }

    public string Exchange { get; }
    public string RoutingKey { get; }
    public Type Type { get; }
    public Func<IServiceProvider, object, object, Task> Handle { get; }

    public static MessageContract Subscribe(Type type, string exchange, string routingKey,
        Func<IServiceProvider, object, object, Task> handle) =>
        new(type, exchange, routingKey, handle);
}