namespace Shared.RabbitMq;

public class RabbitMqMessageProcessingTimeoutException : Exception
{
    public RabbitMqMessageProcessingTimeoutException(string messageId, string correlationId)
        : base(
            $"There was a timeout error when handling the message with ID: '{messageId}', correlation ID: '{correlationId}'.")
    {
        MessageId = messageId;
        CorrelationId = correlationId;
    }

    public string MessageId { get; }
    public string CorrelationId { get; }
}