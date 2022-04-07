namespace Shared.MessageBroker;

public interface IMessageSubscriber
{
    Task SubscribeAsync<T>(Func<IServiceProvider, T, object, Task> handle) where T : class, IMessage;
}