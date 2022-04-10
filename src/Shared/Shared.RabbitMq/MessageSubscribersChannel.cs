﻿using System.Threading.Channels;
using Shared.RabbitMq.Subscribers;

namespace Shared.RabbitMq;

internal class MessageSubscribersChannel
{
    private readonly Channel<IMessageSubscriber> _channel = Channel.CreateUnbounded<IMessageSubscriber>();

    public ChannelReader<IMessageSubscriber> Reader => _channel.Reader;
    public ChannelWriter<IMessageSubscriber> Writer => _channel.Writer;
}