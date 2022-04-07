using Shared.RabbitMq.Conventions;

namespace Shared.RabbitMq.Client;

public interface IRabbitMqClient
{
    public void Send(object message, IConventions conventions, string? messageId = null,
        string? correlationId = null);
}