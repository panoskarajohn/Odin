namespace Shared.RabbitMq.Plugins;

public interface IRabbitMqPluginsRegistry
{
    IRabbitMqPluginsRegistry Add<TPlugin>() where TPlugin : class, IRabbitMqPlugin;
}