namespace Shared.MessageBroker;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message) where T : class, IMessage;
}