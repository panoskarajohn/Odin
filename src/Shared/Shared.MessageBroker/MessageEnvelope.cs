namespace Shared.MessageBroker;

public record MessageEnvelope<T>(T Message, string CorrelationId) where T : IMessage;