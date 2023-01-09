using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;
using Shared.Cqrs.Events;
using Shared.MessageBroker;
using Shared.Web.Context;
using Slip.Application.Events;
using Slip.Application.Exceptions;
using Slip.Core.Repositories;

namespace Slip.Application.Commands.PlaceBet;

public class PlaceBetCommandHandler : ICommandHandler<PlaceBetCommand>
{
    private readonly ILogger<PlaceBetCommandHandler> _logger;
    private readonly ISlipRepository _slipRepository;
    private readonly IContext _context;
    private readonly IBusPublisher _publisher;

    public PlaceBetCommandHandler(ILogger<PlaceBetCommandHandler> logger, ISlipRepository slipRepository, IContext context, IBusPublisher publisher, IEventDispatcher eventDispatcher)
    {
        _logger = logger;
        _slipRepository = slipRepository;
        _context = context;
        _publisher = publisher;
    }
    
    public async Task HandleAsync(PlaceBetCommand command, CancellationToken cancellationToken = default)
    {
        var userId = _context.Identity.Id.ToString();
        var slip = await _slipRepository.GetSlipAsync(userId, cancellationToken);
        
        if (slip is null)
        {
            _logger.LogWarning("Slip not found for user {UserId}", userId);
            throw new SlipNotFoundException();
        }
        
        // Publish a rabbitMq Message
        var message = new UserRequestedBetPlacement(userId, slip);
        await _publisher.PublishAsync(message,Guid.NewGuid().ToString("N"), _context.CorrelationId.ToString());

    }
}