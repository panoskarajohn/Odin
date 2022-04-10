namespace Shared.RabbitMq;

public interface IExceptionToMessageMapper
{
    object Map(Exception exception, object message);
}