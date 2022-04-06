using RabbitMQ.Client;

namespace Shared.RabbitMq.Connections;

public sealed class ProducerConnection
{
    public ProducerConnection(IConnection connection)
    {
        Connection = connection;
    }

    public IConnection Connection { get; }
}