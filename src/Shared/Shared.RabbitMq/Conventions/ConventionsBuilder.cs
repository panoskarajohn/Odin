using System.Reflection;
using Shared.MessageBroker;
using Shared.RabbitMq.Options;

namespace Shared.RabbitMq.Conventions;

public class ConventionsBuilder : IConventionsBuilder
{
    private readonly RabbitMqOptions _options;
    private readonly string _queueTemplate;

    public ConventionsBuilder(RabbitMqOptions options)
    {
        _options = options;
        _queueTemplate = "{{assembly}}/{{exchange}}.{{message}}";
    }

    public string GetRoutingKey(Type type)
    {
        var attribute = GeAttribute(type);
        string routingKey = attribute.RoutingKey;
        return WithCasing(routingKey);
    }

    public string GetExchange(Type type)
    {
        var exchange = string.IsNullOrWhiteSpace(_options.Exchange?.Name)
            ? type.Assembly.GetName().Name
            : _options.Exchange.Name;
        var attribute = GeAttribute(type);
        exchange = string.IsNullOrWhiteSpace(attribute?.Exchange) ? exchange : attribute.Exchange;

        return WithCasing(exchange);
    }

    public string GetQueue(Type type)
    {
        var attribute = GeAttribute(type);
        var assembly = type.Assembly.GetName().Name;
        var message = type.Name;
        var exchange = string.IsNullOrWhiteSpace(attribute?.Exchange)
            ? _options.Exchange?.Name
            : attribute.Exchange;
        var queue = _queueTemplate.Replace("{{assembly}}", assembly)
            .Replace("{{exchange}}", exchange)
            .Replace("{{message}}", message);

        return WithCasing(queue);
    }

    private string WithCasing(string value) => SnakeCase(value);

    private static string SnakeCase(string value)
        => string.Concat(value.Select((x, i) =>
                i > 0 && value[i - 1] != '.' && value[i - 1] != '/' && char.IsUpper(x) ? "_" + x : x.ToString()))
            .ToLowerInvariant();

    private static MessageAttribute GeAttribute(MemberInfo type) => type.GetCustomAttribute<MessageAttribute>()!;
}