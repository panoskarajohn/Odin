using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared.RabbitMq.Connections;
using Shared.RabbitMq.Options;

namespace Shared.RabbitMq.Client;

public class RabbitMqClient : IRabbitMqClient
{
    private readonly ConcurrentDictionary<int, IModel> _channels = new();

    private readonly IConnection _connection;
    private readonly object _lockObject = new();

    private readonly ILogger<RabbitMqClient> _logger;

    private readonly int _maxChannels;

    public RabbitMqClient(RabbitMqOptions options, ProducerConnection connection, ILogger<RabbitMqClient> logger)
    {
        _connection = connection.Connection;
        _logger = logger;
        _maxChannels = options.MaxChannels;
    }

    /// <summary>
    /// Publishes a message to rabbitMa
    /// </summary>
    /// <param name="exchange"></param>
    /// <param name="routingKey"></param>
    /// <param name="message"></param>
    /// <param name="messageId"></param>
    /// <param name="correlationId"></param>
    public void Send(string exchange, string routingKey, object message, string? messageId = null,
        string? correlationId = null)
    {
        var channel = GetChannel();
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.MessageId = string.IsNullOrWhiteSpace(messageId)
            ? Guid.NewGuid().ToString("N")
            : messageId;
        properties.CorrelationId = string.IsNullOrWhiteSpace(correlationId)
            ? Guid.NewGuid().ToString("N")
            : correlationId;
        properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        properties.Headers = new Dictionary<string, object>();

        _logger.LogInformation("Publishing a message with exchange: {Exchange} and Routing Key: {RoutingKey}"
            , exchange, routingKey);

        channel.ExchangeDeclare(exchange, "topic", true, false);
        channel.BasicPublish(exchange, routingKey, properties, body.ToArray());
    }

    /// <summary>
    /// Tries to create a new RabbitMqChannel.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private IModel GetChannel()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        var channelsCount = _channels.Count;

        if (!_channels.TryGetValue(threadId, out var channel))
        {
            lock (_lockObject)
            {
                if (channelsCount >= _maxChannels)
                {
                    throw new InvalidOperationException(
                        $"Cannot create RabbitMq producer channel for thread: {threadId} "
                        + $"reached the limit of {_maxChannels} channels. " +
                        $"Modify {nameof(RabbitMqOptions.MaxChannels)} in rabbitMq configuration");
                }

                channel = _connection.CreateModel();
                _channels.TryAdd(threadId, channel);
                _logger.LogInformation(
                    "Created a channel for threadId: {ThreadId}. Total Channels: {ChannelCount}/{MaxChannels}",
                    threadId, channelsCount, _maxChannels);
            }
        }
        else
        {
            _logger.LogTrace("Reused a channel for thread: {ThreadId}, total channels: {ChannelCount}/{MaxChannels}",
                threadId, channelsCount, _maxChannels);
        }

        return channel;
    }
}