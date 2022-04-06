using System.Threading.Channels;
using Shared.RabbitMq.Subscribers;

namespace Shared.RabbitMq;

internal class MessageSubscribersChannel
{
    private readonly Channel<IMessageContract> _channel = Channel.CreateUnbounded<IMessageContract>();

    public ChannelReader<IMessageContract> Reader => _channel.Reader;
    public ChannelWriter<IMessageContract> Writer => _channel.Writer;
}