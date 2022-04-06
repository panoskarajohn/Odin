using Shared.MessageBroker;
using Shared.RabbitMq.Client;
using Shared.Web.Context;

namespace Shared.RabbitMq.Publishers;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IRabbitMqClient _client;
    private readonly IContext _context;

    public RabbitMqPublisher(IRabbitMqClient client, IContext context)
    {
        _client = client;
        _context = context;
    }

    public Task PublishAsync<T>(string exchange, string routingKey, T message) where T : class, IMessage
    {
        _client.Send(exchange,
            routingKey,
            message,
            _context.RequestId.ToString("N"),
            _context.CorrelationId.ToString("N"));
        return Task.CompletedTask;
    }
}