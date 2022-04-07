namespace Shared.RabbitMq.Conventions;

public class MessageConventions : IConventions
{
    public MessageConventions(Type type, string routingKey, string exchange, string queue)
    {
        Type = type;
        RoutingKey = routingKey;
        Exchange = exchange;
        Queue = queue;
    }

    public Type Type { get; }
    public string RoutingKey { get; }
    public string Exchange { get; }
    public string Queue { get; }
}