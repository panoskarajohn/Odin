namespace Shared.MessageBroker;

public interface IMessagePropertiesAccessor
{
    IMessageProperties MessageProperties { get; set; }
}