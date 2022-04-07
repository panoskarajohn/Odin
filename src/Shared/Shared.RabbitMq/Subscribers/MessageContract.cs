namespace Shared.RabbitMq.Subscribers;

public class MessageContract : IMessageContract
{
    public MessageContract(Type type,
        Func<IServiceProvider, object, object, Task> handle)
    {
        Type = type;
        Handle = handle;
    }

    public Type Type { get; }
    public Func<IServiceProvider, object, object, Task> Handle { get; }

    public static MessageContract Subscribe(Type type,
        Func<IServiceProvider, object, object, Task> handle) =>
        new(type, handle);
}