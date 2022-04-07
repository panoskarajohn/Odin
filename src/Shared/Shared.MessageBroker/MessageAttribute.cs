namespace Shared.MessageBroker;

[AttributeUsage(AttributeTargets.Class)]
public class MessageAttribute : Attribute
{
    public MessageAttribute(string exchange = null, string routingKey = null, string queue = null)
    {
        Exchange = exchange;
        RoutingKey = routingKey;
        Queue = queue;
    }

    public string Exchange { get; }
    public string RoutingKey { get; }
    public string Queue { get; }
}