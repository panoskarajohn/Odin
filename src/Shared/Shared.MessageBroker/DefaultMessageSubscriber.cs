namespace Shared.MessageBroker;

internal sealed class DefaultMessageSubscriber : IMessageSubscriber
{
    public Task SubscribeAsync<T>(string topic, Action<MessageEnvelope<T>> handler) where T : class, IMessage =>
        Task.CompletedTask;
}