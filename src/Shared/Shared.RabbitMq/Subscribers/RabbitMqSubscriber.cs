using Shared.MessageBroker;

namespace Shared.RabbitMq.Subscribers;

internal class RabbitMqSubscriber : IMessageSubscriber
{
    private readonly MessageSubscribersChannel _channel;

    public RabbitMqSubscriber(MessageSubscribersChannel channel)
    {
        _channel = channel;
    }

    public Task SubscribeAsync<T>(Func<IServiceProvider, T, object, Task> handle)
        where T : class, IMessage
    {
        var type = typeof(T);
        var messageContract = MessageContract.Subscribe(type,
            (provider, message, context) => handle(provider, (T) message, context));

        _channel.Writer.TryWrite(messageContract);

        return Task.CompletedTask;
    }
}