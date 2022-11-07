using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Cqrs.Commands;
using Shared.Types.Attributes;
using Shared.Web.Context;

namespace Shared.Logging.CqrsDecorators;

[Decorator]
internal sealed class LoggingCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
{
    private readonly IContext _context;
    private readonly ICommandHandler<T> _handler;
    private readonly ILogger<LoggingCommandHandlerDecorator<T>> _logger;

    public LoggingCommandHandlerDecorator(ILogger<LoggingCommandHandlerDecorator<T>> logger, IContext context,
        ICommandHandler<T> handler)
    {
        _logger = logger;
        _context = context;
        _handler = handler;
    }

    public async Task HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        var name = command.GetType().Name.Underscore();
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity?.Id;
        var correlationId = _context.CorrelationId;

        _logger.LogInformation(
            "Handling a command: {Name} [Request ID: {RequestId}, Corellation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, requestId, correlationId, traceId, userId);
        await _handler.HandleAsync(command, cancellationToken);
        _logger.LogInformation(
            "Handled a command: {Name} [Request ID: {RequestId}, Corellation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, requestId, correlationId, traceId, userId);
    }
}