using RabbitMQ.Client;

namespace Shared.RabbitMq.Connections;

public sealed class ConsumerConnection
{
    public ConsumerConnection(IConnection connection)
    {
        Connection = connection;
    }

    public IConnection Connection { get; }
}