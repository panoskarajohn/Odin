using Shared.MessageBroker;
using Shared.RabbitMq.Client;
using Shared.RabbitMq.Conventions;
using Shared.Web.Context;

namespace Shared.RabbitMq.Publishers;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IRabbitMqClient _client;
    private readonly IContext _context;
    private readonly IConventionsProvider _conventionsProvider;

    public RabbitMqPublisher(IRabbitMqClient client, IContext context, IConventionsProvider conventionsProvider)
    {
        _client = client;
        _context = context;
        _conventionsProvider = conventionsProvider;
    }

    public Task PublishAsync<T>(T message) where T : class, IMessage
    {
        _client.Send(
            message,
            _conventionsProvider.Get(message.GetType()),
            _context.RequestId.ToString("N"),
            _context.CorrelationId.ToString("N"));
        return Task.CompletedTask;
    }
}