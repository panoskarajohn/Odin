namespace Shared.MessageBroker;

internal sealed class DefaultMessageSubscriber : IMessageSubscriber
{
    public Task SubscribeAsync<T>(Func<IServiceProvider, T, object, Task> handle) where T : class, IMessage
        => Task.CompletedTask;
}