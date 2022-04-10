namespace Shared.MessageBroker;

[AttributeUsage(AttributeTargets.Class)]
public class MessageAttribute : Attribute
{
    public MessageAttribute(string exchange = null, string routingKey = null, string queue = null,
        bool external = false)
    {
        Exchange = exchange;
        RoutingKey = routingKey;
        Queue = queue;
        External = external;
    }

    public string Exchange { get; }
    public string RoutingKey { get; }
    public string Queue { get; }
    public bool External { get; }
}