namespace Shared.MessageBroker;

internal sealed class DefaultMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<T>(T message) where T : class, IMessage =>
        Task.CompletedTask;
}