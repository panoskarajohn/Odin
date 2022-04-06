namespace Shared.MessageBroker;

internal sealed class DefaultMessageSubscriber : IMessageSubscriber
{
    public Task SubscribeAsync<T>(string queue, string routingKey, string exchange,
        Func<IServiceProvider, T, object, Task> handle) where T : class, IMessage
        => Task.CompletedTask;
}