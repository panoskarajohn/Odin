namespace Shared.MessageBroker;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message) where T : class, IMessage;
}