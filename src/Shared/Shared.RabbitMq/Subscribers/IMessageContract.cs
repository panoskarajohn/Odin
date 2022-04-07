namespace Shared.RabbitMq.Subscribers;

public interface IMessageContract
{
    Type Type { get; }
    Func<IServiceProvider, object, object, Task> Handle { get; }
}