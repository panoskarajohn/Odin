namespace Shared.RabbitMq;

public interface IExceptionToFailedMessageMapper
{
    FailedMessage Map(Exception exception, object message);
}