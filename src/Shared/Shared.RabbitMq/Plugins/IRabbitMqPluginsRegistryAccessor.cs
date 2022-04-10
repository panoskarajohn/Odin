namespace Shared.RabbitMq.Plugins;

internal interface IRabbitMqPluginsRegistryAccessor
{
    LinkedList<RabbitMqPluginChain> Get();
}