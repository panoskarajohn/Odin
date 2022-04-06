using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Shared.Common;
using Shared.MessageBroker;
using Shared.RabbitMq.Client;
using Shared.RabbitMq.Connections;
using Shared.RabbitMq.Options;
using Shared.RabbitMq.Publishers;

namespace Shared.RabbitMq;

public static class Extensions
{
    private const string RabbitMqSection = "rabbitmq";

    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<RabbitMqOptions>(RabbitMqSection);

        if (options.HostNames is null || !options.HostNames.Any())
        {
            throw new ArgumentException("RabbitMQ hostnames are not specified.", nameof(options.HostNames));
        }

        services.AddSingleton(options);
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddSingleton<IRabbitMqClient, RabbitMqClient>();

        var connectionFactory = new ConnectionFactory
        {
            Port = options.Port,
            VirtualHost = options.VirtualHost,
            UserName = options.Username,
            Password = options.Password,
            DispatchConsumersAsync = true,
        };


        var hostNames = string.Join(", ", options.HostNames);

        var consumerConnection =
            connectionFactory.CreateConnection(options.HostNames.ToList(), $"{options.ConnectionName}_consumer");
        var producerConnection =
            connectionFactory.CreateConnection(options.HostNames.ToList(), $"{options.ConnectionName}_producer");

        services.AddSingleton(new ConsumerConnection(consumerConnection));
        services.AddSingleton(new ProducerConnection(producerConnection));

        return services;
    }
}