namespace Shared.RabbitMq.Subscribers;

internal class MessageSubscriber : IMessageSubscriber
{
    private MessageSubscriber(MessageSubscriberAction action, Type type,
        Func<IServiceProvider, object, object, Task> handle = null)
    {
        Action = action;
        Type = type;
        Handle = handle;
    }

    public MessageSubscriberAction Action { get; }
    public Type Type { get; }
    public Func<IServiceProvider, object, object, Task> Handle { get; }

    public static MessageSubscriber Subscribe(Type type, Func<IServiceProvider, object, object, Task> handle)
    {
        return new(MessageSubscriberAction.Subscribe, type, handle);
    }

    public static MessageSubscriber Unsubscribe(Type type)
    {
        return new(MessageSubscriberAction.Unsubscribe, type);
    }
}