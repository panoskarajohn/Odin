namespace Shared.MessageBroker;

public interface ICorrelationContextAccessor
{
    object CorrelationContext { get; set; }
}