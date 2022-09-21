namespace Shared.RabbitMq;

public interface IContextProvider
{
    string HeaderName { get; }
    object Get(IDictionary<string, object> headers);
}