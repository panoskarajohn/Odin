﻿namespace Shared.RabbitMq.Options;

#nullable disable

/// <summary>
/// This is an options class for setting up the RabbitMQ
/// </summary>
public class RabbitMqOptions
{
    /// <summary>
    /// Max channels that RabbitMq can create
    /// </summary>
    public int MaxChannels { get; set; } = 1000;

    public string ConnectionName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public int Port { get; set; }

    public IEnumerable<string> HostNames { get; set; }
}