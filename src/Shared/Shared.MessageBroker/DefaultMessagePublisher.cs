namespace Shared.MessageBroker;

internal sealed class DefaultMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message) where T : class, IMessage =>
        Task.CompletedTask;
}