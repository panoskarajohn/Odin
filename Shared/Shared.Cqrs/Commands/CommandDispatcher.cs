using Microsoft.Extensions.DependencyInjection;

namespace Shared.Cqrs.Commands;

internal sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
        await handler.HandleAsync(command, cancellationToken);
    }
}