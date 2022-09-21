using System.Collections.Concurrent;

namespace Shared.RabbitMq.Conventions;

public class ConventionsProvider : IConventionsProvider
{
    private readonly IConventionsBuilder _builder;

    private readonly ConcurrentDictionary<Type, IConventions> _conventions =
        new();

    public ConventionsProvider(IConventionsBuilder builder)
    {
        _builder = builder;
    }

    public IConventions Get<T>() => Get(typeof(T));

    public IConventions Get(Type type)
    {
        if (_conventions.TryGetValue(type, out var conventions))
        {
            return conventions;
        }

        conventions = new MessageConventions(type, _builder.GetRoutingKey(type),
            _builder.GetExchange(type), _builder.GetQueue(type));

        _conventions.TryAdd(type, conventions);

        return conventions;
    }
}