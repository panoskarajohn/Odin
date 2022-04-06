namespace Shared.MessageBroker;

public interface IMessageSubscriber
{
    Task SubscribeAsync<T>(string queue, string routingKey, string exchange,
        Func<IServiceProvider, T, object, Task> handle) where T : class, IMessage;
}