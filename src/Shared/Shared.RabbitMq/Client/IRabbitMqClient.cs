namespace Shared.RabbitMq.Client;

public interface IRabbitMqClient
{
    public void Send(string exchange, string routingKey, object message, string? messageId = null,
        string? correlationId = null);
}